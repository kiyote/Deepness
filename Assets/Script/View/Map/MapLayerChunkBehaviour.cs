using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public delegate void MapLayerChunkClickHandler(Vector3 worldPosition);

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(MeshFilter))]
public class MapLayerChunkBehaviour : MonoBehaviour, IPointerClickHandler {
	
	public const int BlockSize = 10;

	private List<Vector3> _vertices;
	private List<int> _triangles;
	private List<Vector3> _normals;
	private List<Vector2> _uv;
	private List<Color32> _colors;

	private Vector3 _normal;

	public event MapLayerChunkClickHandler Clicked;

	public MapLayerChunkBehaviour(): base()
	{
		_vertices = new List<Vector3>();
		_triangles = new List<int>();
		_normals = new List<Vector3>();
		_uv = new List<Vector2>();
		_colors = new List<Color32>();
		_normal = new Vector3(0, 0, -1);
	}

	public void Populate(Map map, int startColumn, int startRow, TerrainParser tp)
	{
		GenerateMesh(map, startColumn, startRow, tp);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId == PointerInputModule.kMouseLeftId)
		{
			if (Clicked != null)
			{
				Clicked(eventData.worldPosition);
			}
		}
		else if (eventData.pointerId == PointerInputModule.kMouseRightId)
		{
			Debug.Log (eventData);
		}
	}

	private void GenerateMesh(Map map, int startColumn, int startRow, TerrainParser tp)
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.sharedMaterial = tp.TerrainMaterial;

		_vertices.Clear();
		_triangles.Clear();
		_normals.Clear();
		_uv.Clear();
		_colors.Clear();

		for (int r = 0; r < BlockSize; r++)
		{
			for (int c = 0; c < BlockSize; c++)
			{
				int x = c + startColumn;
				int y = r + startRow;

				if ((x >= 0) && (x < map.Width) && (y >= 0) && (y < map.Height))
				{
					MapTile mapTile = map.Tile[x, y];
					
					if (mapTile != null)
					{
						Rect uvc = tp.Definition[tp.Terrain["dirt"]].Floor;
						GenerateTile(c, r, ref uvc);
						
						if (mapTile.IsWall)
						{
							if (tp.Definition[tp.Terrain["grass"]].Fringe.ContainsKey((int)mapTile.Fringe)) 
							{
								uvc = tp.Definition[tp.Terrain["grass"]].Fringe[(int)mapTile.Fringe];
								GenerateTile(c, r, ref uvc);
							}
						}
					}
				}
			}
		}
		
		Mesh mesh = new Mesh();
		mesh.vertices = _vertices.ToArray();
		mesh.colors32 = _colors.ToArray();
		mesh.triangles = _triangles.ToArray();
		mesh.normals = _normals.ToArray();
		mesh.uv = _uv.ToArray();

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		Destroy(meshFilter.sharedMesh);
		meshFilter.mesh = mesh;
	}

	private void GenerateTile(int c, int r, ref Rect uvc)
	{
		int vertexIndex = _vertices.Count;
		_vertices.Add(new Vector3(c + -0.5f, r + -0.5f, 0));
		_vertices.Add(new Vector3(c + 0.5f, r + -0.5f, 0));
		_vertices.Add(new Vector3(c + 0.5f, r + 0.5f, 0));
		_vertices.Add(new Vector3(c + -0.5f, r + 0.5f, 0));
		
		_triangles.Add(vertexIndex + 3);
		_triangles.Add(vertexIndex + 2);
		_triangles.Add(vertexIndex + 1);
		_triangles.Add(vertexIndex + 1);
		_triangles.Add(vertexIndex + 0);
		_triangles.Add(vertexIndex + 3);
		
		_normals.Add(_normal);
		_normals.Add(_normal);
		_normals.Add(_normal);
		_normals.Add(_normal);
		
		_uv.Add(new Vector2(uvc.x, uvc.y));
		_uv.Add(new Vector2(uvc.width, uvc.y));
		_uv.Add(new Vector2(uvc.width, uvc.height));
		_uv.Add(new Vector2(uvc.x, uvc.height));

		_colors.Add(Color.white);
		_colors.Add(Color.white);
		_colors.Add(Color.white);
		_colors.Add(Color.white);
	}
}
