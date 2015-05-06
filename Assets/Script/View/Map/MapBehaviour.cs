﻿using UnityEngine;
using System.Collections.Generic;

public class MapBehaviour : MonoBehaviour {

	public const string MapObjectName = "Map";
	public const string TerrainLayerObjectName = MapObjectName + "/" + "Terrain";
	public const string BlockCacheObjectName = TerrainLayerObjectName + "/" + "BlockCache";
	public const int TileSize = 24;

	private class IndexedBlock
	{
		public IndexedBlock(int column, int row, GameObject gameObject)
		{
			Column = column;
			Row = row;
			GameObject = gameObject;
			Used = true;
		}

		public int Column;
		public int Row;
		public GameObject GameObject;
		public bool Used;
		public bool Dirty;
	}

	public GameObject MapLayerChunkPrefab;

	// Fast references to objects we need, and pretty much own
	private GameObject _blockCache;
	private List<GameObject> _cachedBlocks;
	private List<IndexedBlock> _onDisplay;

	// These are local caches of other objects
	private Map _map;
	private TerrainParser _tp;
	private int _rOffset;
	private int _cOffset;
	private GameObject _mapObject;
	private GameObject _terrainObject;

	void Start()
	{
        SceneEvents.CreatingMap += CreateMap;

		Camera.main.GetComponent<CameraBehaviour>().CameraUpdated += CameraUpdated;
		_blockCache = GameObject.Find(BlockCacheObjectName);

		_cachedBlocks = new List<GameObject>();
		_onDisplay = new List<IndexedBlock>();
	}

	void Update()
	{
		for (int i = 0; i < _onDisplay.Count; i++)
		{
			IndexedBlock block = _onDisplay[i];
			if (block.Dirty)
			{
				block.Dirty = false;
				MapLayerChunkBehaviour mapLayerChunkBehaviour = block.GameObject.GetComponent<MapLayerChunkBehaviour>();
				mapLayerChunkBehaviour.Populate(_map, block.Column, block.Row, _tp);
			}
		}
	}

	private void ChunkClicked(Vector3 worldPosition)
	{
		float x = (worldPosition.x + (_map.Width / 2.0f));
		float y = (worldPosition.y + (_map.Height / 2.0f));
		int column = Mathf.RoundToInt(x);
		int row = Mathf.RoundToInt(y);

		_map.BeginUpdate();
		_map.Tile[column, row].IsWall = false;
		_map.EndUpdate();
	}
	
	public MapTile GetMapTile(Vector3 position)
	{
		return null;
	}

    public void CreateMap(object sender, CreateMapEventArgs e)
    {
        Populate(e.Map);
    }

	private void Populate(Map map)
	{
		if (_map != null)
		{
			_map.MapChanged -= MapChanged;
			_map.MapBatchChanged -= MapBatchChanged;
		}
		_map = map;
		if (_map != null)
		{
			_map.MapChanged += MapChanged;
			_map.MapBatchChanged += MapBatchChanged;
			_rOffset = map.Height / 2;
			_cOffset = map.Width / 2;
		}
		else
		{
			_rOffset = 0;
			_cOffset = 0;
		}

		GameObject mapObject = GameObject.Find(MapObjectName);
		if (mapObject != _mapObject)
		{
			_mapObject = mapObject;
			if (mapObject != null)
			{
				_tp = mapObject.GetComponent<TerrainParser>();
				_terrainObject = GameObject.Find(TerrainLayerObjectName);
			}
			else
			{
				_tp = null;
				_terrainObject = null;
			}
		}

		CalculateBlocksOnDisplay(Camera.main.OrthographicBounds());
	}

	private void MapChanged(Map map, int column, int row, MapTile mapTile)
	{
		IndexedBlock ib = GetMapBlock(column, row);
		MapLayerChunkBehaviour mapChunk = ib.GameObject.GetComponent<MapLayerChunkBehaviour>();
		mapChunk.Populate(map, ib.Column, ib.Row, _tp);
	}

	private void MapBatchChanged(Map map, List<MapTile> dirtyTiles)
	{
		for (int i = 0; i < dirtyTiles.Count; i++)
		{
			MapTile mapTile = dirtyTiles[i];
			IndexedBlock block = GetMapBlock(mapTile.Column, mapTile.Row);
			if (block != null)
			{
				block.Dirty = true;
			}
		}

		for (int i = 0; i < _onDisplay.Count; i++)
		{
			IndexedBlock ib = _onDisplay[i];
			if (ib.Dirty)
			{
				MapLayerChunkBehaviour mapChunk = ib.GameObject.GetComponent<MapLayerChunkBehaviour>();
				mapChunk.Populate(map, ib.Column, ib.Row, _tp);
			}
		}
	}

	private void CameraUpdated(Camera sender, CameraUpdateEventArgs e)
	{
		if (_map != null)
		{
			CalculateBlocksOnDisplay(e.Bounds);
		}
	}

	IndexedBlock GetMapBlock(int column, int row)
	{
		for (int i = 0; i < _onDisplay.Count; i++)
		{
			IndexedBlock ib = _onDisplay[i];
			if ((ib.Column <= column) && (ib.Column + MapLayerChunkBehaviour.BlockSize > column) &&
				(ib.Row <= row) && (ib.Row + MapLayerChunkBehaviour.BlockSize > row))
			{
				return ib;
			}
		}

		return null;
	}

	private void CalculateBlocksOnDisplay(Rect bounds)
	{
		float blockSize = (float)MapLayerChunkBehaviour.BlockSize;

		int startCol = (int)(bounds.x / blockSize) - 1; // Offset by 1 to the left
		int endCol = startCol + (int)(bounds.width / blockSize) + 2; // Offset by 1 to the right (2 because we did one to the left)
		
		int startRow = (int)(bounds.y / blockSize) - 1;
		int endRow = startRow + (int)(bounds.height / blockSize) + 2;

		startCol *= MapLayerChunkBehaviour.BlockSize;
		endCol *= MapLayerChunkBehaviour.BlockSize;
		startRow *= MapLayerChunkBehaviour.BlockSize;
		endRow *= MapLayerChunkBehaviour.BlockSize;

		int halfWidth = (_map.Width / 2);
		startCol += halfWidth;
		endCol += halfWidth;

		int halfHeight = (_map.Height / 2);
		startRow += halfHeight;
		endRow += halfHeight;

		// Mark every block as not being on display
		for (int i = 0; i < _onDisplay.Count; i++)
		{
			_onDisplay[i].Used = false;
		}

		for (int r = startRow; r <= endRow; r += MapLayerChunkBehaviour.BlockSize)
		{
			for (int c = startCol; c <= endCol; c += MapLayerChunkBehaviour.BlockSize)
			{
				// Really inefficiently find the block we're interested in....
				bool found = false;
				for (int i = 0; i < _onDisplay.Count; i++)
				{
					IndexedBlock ib = _onDisplay[i];
					if ((ib.Column == c) && (ib.Row == r))
					{
						// If the row and column match, it's on display so mark it as in use
						ib.Used = true;
						found = true;
					}
				}

				// If we didn't find a block and we're processing within the bounds of the map
				// we need to make a new block (or recycle a cached block) and put it on display
				if ((!found) && (r >= 0) && (r < _map.Height) && (c >= 0) && (c < _map.Width))
				{
					GameObject go = CreateChunk(_terrainObject, _map, c, r, _cOffset, _rOffset, _tp);
					_onDisplay.Add(new IndexedBlock(c, r, go));
				}
			}
		}

		// Now find all of the unused blocks
		List<IndexedBlock> unused = new List<IndexedBlock>();
		for (int i = 0; i < _onDisplay.Count; i++)
		{
			IndexedBlock ib = _onDisplay[i];
			if (!ib.Used)
			{
				unused.Add(ib);
			}
		}

		// Move all the unused blocks to the block cache
		for (int i = 0; i < unused.Count; i++)
		{
			IndexedBlock ib = unused[i];
			_onDisplay.Remove(ib);
			ib.GameObject.transform.parent = _blockCache.transform;
			ib.GameObject.SetActive(false);
			_cachedBlocks.Add(ib.GameObject);
		}
	}

	private GameObject CreateChunk(GameObject terrainObject, Map map, int column, int row, int columnOffset, int rowOffset, TerrainParser tp)
	{
		GameObject mapLayerChunkObject = GetNewMapBlock();
		mapLayerChunkObject.transform.parent = terrainObject.transform;
		mapLayerChunkObject.transform.localPosition = new Vector3(column - columnOffset, row - rowOffset);
		MapLayerChunkBehaviour mapLayerChunkBehaviour = mapLayerChunkObject.GetComponent<MapLayerChunkBehaviour>();
		mapLayerChunkBehaviour.Populate(map, column, row, tp);

		return mapLayerChunkObject;
	}

	private GameObject GetNewMapBlock()
	{
		GameObject result;
		
		if (_cachedBlocks.Count > 0) 
		{
			result = _cachedBlocks[0];
			result.SetActive(true);
			_cachedBlocks.RemoveAt(0);
		}
		else
		{
			result = (GameObject)Instantiate(MapLayerChunkPrefab, Vector3.zero, Quaternion.identity);
			MapLayerChunkBehaviour mapLayerChunkBehaviour = result.GetComponent<MapLayerChunkBehaviour>();
			mapLayerChunkBehaviour.Clicked += ChunkClicked;
		}
		
		return result;
	}
}