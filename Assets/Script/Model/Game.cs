using System;
using System.Collections.Generic;

public class Game
{
	private static Game _instance = new Game();

	private Map _map;
    private TerrainDefinition _terrain;

	public Game ()
	{
		_map = new Map();
        _terrain = new TerrainDefinition();
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

}

