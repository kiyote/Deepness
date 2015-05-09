using UnityEngine;
using System;
using System.Collections.Generic;

public class TerrainTileDefinition
{
    private Rect _floor;
    private Dictionary<int, Rect> _fringe;

    public TerrainTileDefinition()
    {
        _fringe = new Dictionary<int, Rect>();
    }

    public Rect Floor
    {
        get
        {
            return _floor;
        }
        set
        {
            _floor = value;
        }
    }

    public Dictionary<int, Rect> Fringe
    {
        get
        {
            return _fringe;
        }
    }
}

public class TerrainTextureDefinition
{
    private Dictionary<Terrain, TerrainTileDefinition> _definitionByTerrain;
    private Material _material;

    public TerrainTextureDefinition(Material material)
    {
        _material = material;
        _definitionByTerrain = new Dictionary<Terrain, TerrainTileDefinition>();
    }

    public Material Material
    {
        get
        {
            return _material;
        }
    }

    public TerrainTileDefinition Create(Terrain terrain)
    {
        TerrainTileDefinition definition = new TerrainTileDefinition();
        _definitionByTerrain[terrain] = definition;

        return definition;
    }

    public TerrainTileDefinition ByTerrain(Terrain terrain)
    {
        if (_definitionByTerrain.ContainsKey(terrain))
        {
            return _definitionByTerrain[terrain];
        }
        return null;
    }
}
