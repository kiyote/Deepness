using System;
using System.Collections.Generic;

public class TerrainDefinition
{
    private IDictionary<int, Terrain> _terrainById;
    private IDictionary<string, Terrain> _terrainByName;

    public TerrainDefinition()
    {
        _terrainById = new Dictionary<int, Terrain>();
        _terrainByName = new Dictionary<string, Terrain>();
    }

    public void Add(Terrain terrain)
    {
        _terrainById[terrain.Id] = terrain;
        _terrainByName[terrain.Name] = terrain;
    }

    public Terrain ByName(string name)
    {
        if (_terrainByName.ContainsKey(name) == false)
        {
            throw new InvalidOperationException(String.Format("Unable to locate terrain '{0}'.", name));
        }
        return _terrainByName[name];
    }

    public Terrain ById(int id)
    {
        return _terrainById[id];
    }

    public ICollection<Terrain> Terrain
    {
        get
        {
            return _terrainById.Values;
        }
    }
}
