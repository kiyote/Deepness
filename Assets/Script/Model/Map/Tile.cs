
namespace Model.Map
{
    using System.Collections.Generic;

    public class Tile
    {
        private class TileTerrain
        {
            public TileTerrain(MapTerrain terrain)
            {
                Edges = TileCompass.None;
                Terrain = terrain;
            }

            public TileCompass Edges;
            public MapTerrain Terrain;
        }

        private Map _map;
        private int _column;
        private int _row;
        private bool _isWall;
        private MapTerrain _terrain;
        private Dictionary<MapTerrain, TileTerrain> _fringe;
        private Dictionary<MapTerrain, TileTerrain> _walls;

        public Tile(Map map, int column, int row)
        {
            _map = map;
            _column = column;
            _row = row;
            _isWall = false;
            _fringe = new Dictionary<MapTerrain, TileTerrain>();
            _walls = new Dictionary<MapTerrain, TileTerrain>();
        }

        public MapTerrain Terrain
        {
            get
            {
                return _terrain;
            }
            set
            {
                _terrain = value;
            }
        }

        public int Column
        {
            get
            {
                return _column;
            }
        }

        public int Row
        {
            get
            {
                return _row;
            }
        }

        public bool IsWall
        {
            get
            {
                return _isWall;
            }
            set
            {
                if (value != _isWall)
                {
                    _isWall = value;
                    _map.SignalMapChanged(this);
                }
            }
        }

        public void SetFringe(MapTerrain terrain, TileCompass fringe)
        {
            TileTerrain tileFringe;
            if (_fringe.ContainsKey(terrain))
            {
                tileFringe = _fringe[terrain];
            }
            else
            {
                tileFringe = new TileTerrain(terrain);
                _fringe[terrain] = tileFringe;
            }

            tileFringe.Edges = fringe;
        }

        public void SetWalls(MapTerrain terrain, TileCompass sides)
        {
            TileTerrain tileWalls;
            if (_walls.ContainsKey(terrain))
            {
                tileWalls = _walls[terrain];
            }
            else
            {
                tileWalls = new TileTerrain(terrain);
                _walls[terrain] = tileWalls;
            }

            tileWalls.Edges = sides;
        }

        public void AddFringe(MapTerrain terrain, TileCompass fringe)
        {
            TileTerrain tileFringe;
            if (_fringe.ContainsKey(terrain))
            {
                tileFringe = _fringe[terrain];
            }
            else
            {
                tileFringe = new TileTerrain(terrain);
                _fringe[terrain] = tileFringe;
            }

            tileFringe.Edges |= fringe;
        }

        public void AddWall(MapTerrain terrain, TileCompass edge)
        {
            TileTerrain tileWalls;
            if (_walls.ContainsKey(terrain))
            {
                tileWalls = _walls[terrain];
            }
            else
            {
                tileWalls = new TileTerrain(terrain);
                _walls[terrain] = tileWalls;
            }

            tileWalls.Edges |= edge;
        }

        public void RemoveWall(MapTerrain terrain, TileCompass edge)
        {
            TileTerrain tileWalls;
            if (_walls.ContainsKey(terrain))
            {
                tileWalls = _walls[terrain];
            }
            else
            {
                tileWalls = new TileTerrain(terrain);
                _walls[terrain] = tileWalls;
            }

            tileWalls.Edges &= ~edge;
        }

        public TileCompass GetFringe(MapTerrain terrain)
        {
            if (_fringe.ContainsKey(terrain))
            {
                return _fringe[terrain].Edges;
            }
            else
            {
                return TileCompass.None;
            }
        }

        public TileCompass GetWalls(MapTerrain terrain)
        {
            if (_walls.ContainsKey(terrain))
            {
                return _walls[terrain].Edges;
            }
            else
            {
                return TileCompass.None;
            }
        }
    }

}

