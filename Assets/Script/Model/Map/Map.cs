
namespace Model.Map
{
    using System.Collections.Generic;

    public delegate void MapChangedEventHandler(Map map, int column, int row, MapTile mapTile);
    public delegate void MapBatchChangedEventHandler(Map map, List<MapTile> dirtyTiles);

    public class Map
    {
        public const int TileSize = 24;

        private int _width;
        private int _height;
        private MapTile[,] _tiles;

        public event MapChangedEventHandler MapChanged;
        public event MapBatchChangedEventHandler MapBatchChanged;
        private bool _inBatch;
        private List<MapTile> _dirtyTiles;
        private bool _creating;

        public Map()
        {
            _inBatch = false;
            _dirtyTiles = new List<MapTile>();
            _creating = false;
        }

        public void Create(int tileWidth, int tileHeight)
        {
            _width = tileWidth;
            _height = tileHeight;
            _tiles = new MapTile[tileWidth, tileHeight];

            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    _tiles[x, y] = new MapTile(this, x, y);
                }
            }
        }

        #region Map Updating Event Logic
        public void BeginCreate()
        {
            _creating = true;
        }

        public void EndCreate()
        {
            CompileMap();
            _creating = false;
        }

        public void BeginUpdate()
        {
            _dirtyTiles.Clear();
            _inBatch = true;
        }

        public void SignalMapChanged(MapTile mapTile)
        {
            if (_creating)
            {
                return;
            }

            if (_inBatch)
            {
                if (_dirtyTiles.Contains(mapTile) == false)
                {
                    _dirtyTiles.Add(mapTile);
                }
                return;
            }

            if ((mapTile != null) && (MapChanged != null))
            {
                CompileMap();
                MapChanged(this, mapTile.Column, mapTile.Row, mapTile);
            }
        }

        public void EndUpdate()
        {
            CompileMap();
            _inBatch = false;
            if ((_dirtyTiles.Count > 0) && (MapBatchChanged != null))
            {
                MapBatchChanged(this, _dirtyTiles);
            }
        }
        #endregion

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
        }

        public MapTile[,] Tile
        {
            get
            {
                return _tiles;
            }
        }


        private MapTile GetTile(int col, int row)
        {
            if ((col >= 0) && (col < _width) && (row >= 0) && (row < _height))
            {
                return _tiles[col, row];
            }
            else
            {
                return null;
            }
        }

        public void CompileMap()
        {
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    MapTile tile = _tiles[col, row];

                    MapTile up = GetTile(col, row + 1);
                    MapTile left = GetTile(col - 1, row);
                    MapTile right = GetTile(col + 1, row);
                    MapTile down = GetTile(col, row - 1);

                    if ((up != null) && (up.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(up.Terrain, TileCompass.Top);
                    }

                    if ((right != null) && (right.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(right.Terrain, TileCompass.Right);
                    }

                    if ((down != null) && (down.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(down.Terrain, TileCompass.Bottom);
                    }

                    if ((left != null) && (left.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(left.Terrain, TileCompass.Left);
                    }

                    if (tile.IsWall)
                    {
                        MapTile upLeft = GetTile(col - 1, row + 1);
                        MapTile upRight = GetTile(col + 1, row + 1);
                        MapTile downLeft = GetTile(col - 1, row - 1);
                        MapTile downRight = GetTile(col + 1, row - 1);

                        if ((up != null) && (up.IsWall && tile.IsWall) && (up.Terrain.Id == tile.Terrain.Id))
                        {
                            tile.AddWall(tile.Terrain, TileCompass.Top);
                        }

                        if ((right != null) && (right.IsWall && tile.IsWall) && (right.Terrain.Id == tile.Terrain.Id))
                        {
                            tile.AddWall(tile.Terrain, TileCompass.Right);
                        }

                        if ((down != null) && (down.IsWall && tile.IsWall) && (down.Terrain.Id == tile.Terrain.Id))
                        {
                            tile.AddWall(tile.Terrain, TileCompass.Bottom);
                        }

                        if ((left != null) && (left.IsWall && tile.IsWall) && (left.Terrain.Id == tile.Terrain.Id))
                        {
                            tile.AddWall(tile.Terrain, TileCompass.Left);
                        }

                        if ((upLeft != null) && (upLeft.IsWall && tile.IsWall) && (upLeft.Terrain.Id == tile.Terrain.Id))
                        {
                            if ((up != null) && (left != null) && (up.IsWall) && (left.IsWall) && (left.Terrain.Id == tile.Terrain.Id) && (up.Terrain.Id == tile.Terrain.Id))
                            {
                                tile.AddWall(tile.Terrain, TileCompass.TopLeft);
                            }
                        }

                        if ((upRight != null) && (upRight.IsWall && tile.IsWall) && (upRight.Terrain.Id == tile.Terrain.Id))
                        {
                            if ((up != null) && (right != null) && (up.IsWall) && (right.IsWall) && (right.Terrain.Id == tile.Terrain.Id) && (up.Terrain.Id == tile.Terrain.Id))
                            {
                                tile.AddWall(tile.Terrain, TileCompass.TopRight);
                            }
                        }

                        if ((downLeft != null) && (downLeft.IsWall && tile.IsWall) && (downLeft.Terrain.Id == tile.Terrain.Id))
                        {
                            if ((down != null) && (left != null) && (down.IsWall) && (left.IsWall) && (down.Terrain.Id == tile.Terrain.Id) && (left.Terrain.Id == tile.Terrain.Id))
                            {
                                tile.AddWall(tile.Terrain, TileCompass.BottomLeft);
                            }
                        }

                        if ((downRight != null) && (downRight.IsWall && tile.IsWall) && (downRight.Terrain.Id == tile.Terrain.Id))
                        {
                            if ((down != null) && (right != null) && (down.IsWall) && (right.IsWall) && (down.Terrain.Id == tile.Terrain.Id) && (right.Terrain.Id == tile.Terrain.Id))
                            {
                                tile.AddWall(tile.Terrain, TileCompass.BottomRight);
                            }
                        }

                    }


                    //if (oldWalls != tileFringe)
                    //{
                        SignalMapChanged(tile);
                    //}
                }
            }

            /*
            for (int y = 0; y < _height; y++ )
            {
                for (int x = 0; x < _width; x++)
                {
                    MapTile tile = _tiles[x, y];
				
				
                    foreach (KeyValuePair<Terrain, Fringe> kvp in tile.Fringe)
                    {
                        Terrain t = kvp.Key;
                        Fringe f = kvp.Value;
					
                        if ((f.Edge & Edge.Top) == Edge.Top)
                        {
                            f.Corner &= ~(Corner.TopLeft | Corner.TopRight);
                        }
                        if ((f.Edge & Edge.Right) == Edge.Right)
                        {
                            f.Corner &= ~(Corner.TopRight | Corner.BottomRight);
                        }
                        if ((f.Edge & Edge.Bottom) == Edge.Bottom)
                        {
                            f.Corner &= ~(Corner.BottomLeft | Corner.BottomRight);
                        }
                        if ((f.Edge & Edge.Left) == Edge.Left)
                        {
                            f.Corner &= ~(Corner.TopLeft | Corner.BottomLeft);
                        }
                    }
				
                    List<Terrain> s = new List<Terrain>(tile.Fringe.Keys);
                    s.Sort((a, b) => a.Id.CompareTo(b.Id));
				
                    foreach (Terrain t in s)
                    {
                        Edge e = tile.Fringe[t].Edge;
                        if (e != Edge.None)
                        {
                            tile.FringeImage.Add(t.Edge[e]);
                        }
                        Corner c = tile.Fringe[t].Corner;
                        if (c != Corner.None)
                        {
                            tile.FringeImage.Add(t.Corner[c]);
                        }
                    }
				
                }
            }
            */

        }

        /*
        Fringe GetFringe(MapTile tile, Terrain terrain)
        {
            if (tile.Fringe.ContainsKey(terrain) == false)
            {
                tile.Fringe[terrain] = new Fringe();
            }
            return tile.Fringe[terrain];
        }
        */

        int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }


    }

}
