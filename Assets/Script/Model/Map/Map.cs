
namespace Model.Map
{
    using System.Collections.Generic;

    public delegate void MapChangedEventHandler(Map map, int column, int row, Tile mapTile);
    public delegate void MapBatchChangedEventHandler(Map map, List<Tile> dirtyTiles);

    public class Map
    {
        public const int TileSize = 24;

        private int _width;
        private int _height;
        private Tile[,] _tiles;

        public event MapChangedEventHandler MapChanged;
        public event MapBatchChangedEventHandler MapBatchChanged;
        private bool _inBatch;
        private List<Tile> _dirtyTiles;
        private bool _creating;

        public Map()
        {
            _inBatch = false;
            _dirtyTiles = new List<Tile>();
            _creating = false;
        }

        public void Create(int tileWidth, int tileHeight)
        {
            _width = tileWidth;
            _height = tileHeight;
            _tiles = new Tile[tileWidth, tileHeight];

            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    _tiles[x, y] = new Tile(this, x, y);
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

        public void SignalMapChanged(Tile mapTile)
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

        public Tile[,] Tile
        {
            get
            {
                return _tiles;
            }
        }


        private Tile GetTile(int col, int row)
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
                    Tile tile = _tiles[col, row];

                    Tile up = GetTile(col, row + 1);
                    Tile left = GetTile(col - 1, row);
                    Tile right = GetTile(col + 1, row);
                    Tile down = GetTile(col, row - 1);
                    Tile upLeft = GetTile(col - 1, row + 1);
                    Tile upRight = GetTile(col + 1, row + 1);
                    Tile downLeft = GetTile(col - 1, row - 1);
                    Tile downRight = GetTile(col + 1, row - 1);

                    if ((upLeft != null) && (upLeft.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(upLeft.Terrain, TileCompass.TopLeft);
                    }

                    if ((upRight != null) && (upRight.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(upRight.Terrain, TileCompass.TopRight);
                    }

                    if ((downLeft != null) && (downLeft.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(downLeft.Terrain, TileCompass.BottomLeft);
                    }

                    if ((downRight != null) && (downRight.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(downRight.Terrain, TileCompass.BottomRight);
                    }

                    if ((up != null) && (up.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(up.Terrain, TileCompass.Top);

                        // Mask out the corners
                        tile.SetFringe(up.Terrain, tile.GetFringe(up.Terrain) & ~(TileCompass.TopLeft | TileCompass.TopRight));
                    }

                    if ((right != null) && (right.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(right.Terrain, TileCompass.Right);

                        // Mask out the corners
                        tile.SetFringe(right.Terrain, tile.GetFringe(right.Terrain) & ~(TileCompass.BottomRight | TileCompass.TopRight));
                    }

                    if ((down != null) && (down.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(down.Terrain, TileCompass.Bottom);

                        // Mask out the corners
                        tile.SetFringe(down.Terrain, tile.GetFringe(down.Terrain) & ~(TileCompass.BottomLeft | TileCompass.BottomRight));
                    }

                    if ((left != null) && (left.Terrain.Id > tile.Terrain.Id))
                    {
                        tile.AddFringe(left.Terrain, TileCompass.Left);

                        // Mask out the corners
                        tile.SetFringe(left.Terrain, tile.GetFringe(left.Terrain) & ~(TileCompass.TopLeft | TileCompass.BottomLeft));
                    }

                    if (tile.IsWall)
                    {
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
        }

        int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}
