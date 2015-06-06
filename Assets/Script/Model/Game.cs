using System;
using System.Collections.Generic;
using Model.Map;

public class Game
{
	private static Game _instance = new Game();

    private Map _map;
    private TerrainDefinition _terrain;
    private FeatureDefinition _feature;

	public Game ()
	{
        _map = new Map();
        _terrain = new TerrainDefinition();
        _feature = new FeatureDefinition();
	}

	public static Game Instance
	{
        get
        {
            return _instance;
        }
	}

    public Map Map
	{
		get
		{
			return _map;
		}
	}

    public TerrainDefinition Terrain
    {
        get
        {
            return _terrain;
        }
    }

    public FeatureDefinition Feature
    {
        get
        {
            return _feature;
        }
    }

}

