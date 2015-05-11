
namespace Model.Map
{
    using System;
    using System.Collections.Generic;

    public class TerrainDefinition
    {
        private IDictionary<int, MapTerrain> _terrainById;
        private IDictionary<string, MapTerrain> _terrainByName;

        public TerrainDefinition()
        {
            _terrainById = new Dictionary<int, MapTerrain>();
            _terrainByName = new Dictionary<string, MapTerrain>();
        }

        public void Add(MapTerrain terrain)
        {
            _terrainById[terrain.Id] = terrain;
            _terrainByName[terrain.Name] = terrain;
        }

        public MapTerrain ByName(string name)
        {
            if (_terrainByName.ContainsKey(name) == false)
            {
                throw new InvalidOperationException(String.Format("Unable to locate terrain '{0}'.", name));
            }
            return _terrainByName[name];
        }

        public MapTerrain ById(int id)
        {
            return _terrainById[id];
        }

        public ICollection<MapTerrain> Terrain
        {
            get
            {
                return _terrainById.Values;
            }
        }
    }
}

