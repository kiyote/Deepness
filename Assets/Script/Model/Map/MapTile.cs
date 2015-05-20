
namespace Model.Map
{
    using System.Collections.Generic;

    public class Fringe
    {
        public Edge Edge;
        public Corner Corner;
    }

    public class MapTile
    {
        private class TileFringe
        {
            public TileFringe(MapTerrain terrain)
            {
                Fringe = TileCompass.None;
                Terrain = terrain;
            }

            public TileCompass Fringe;
            public MapTerrain Terrain;
        }

        private Map _map;
        private int _column;
        private int _row;
        private bool _isWall;
        private MapTerrain _terrain;
        private Dictionary<MapTerrain, TileFringe> _fringe;

        public MapTile(Map map, int column, int row)
        {
            _map = map;
            _column = column;
            _row = row;
            _isWall = false;
            _fringe = new Dictionary<MapTerrain, TileFringe>();
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
            TileFringe tileFringe;
            if (_fringe.ContainsKey(terrain))
            {
                tileFringe = _fringe[terrain];
            }
            else
            {
                tileFringe = new TileFringe(terrain);
                _fringe[terrain] = tileFringe;
            }

            tileFringe.Fringe = fringe;
        }

        public void AddFringe(MapTerrain terrain, TileCompass fringe)
        {
            TileFringe tileFringe;
            if (_fringe.ContainsKey(terrain))
            {
                tileFringe = _fringe[terrain];
            }
            else
            {
                tileFringe = new TileFringe(terrain);
                _fringe[terrain] = tileFringe;
            }

            tileFringe.Fringe |= fringe;
        }

        public TileCompass GetFringe(MapTerrain terrain)
        {
            if (_fringe.ContainsKey(terrain))
            {
                return _fringe[terrain].Fringe;
            }
            else
            {
                return TileCompass.None;
            }
        }
    }

}

