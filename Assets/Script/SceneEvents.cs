using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameEventArgs: EventArgs
{
    public CreateGameEventArgs(int mapWidth, int mapHeight)
    {
        MapWidth = mapWidth;
        MapHeight = mapHeight;
    }

    public int MapWidth;
    public int MapHeight;
}

public class CreateMapEventArgs: EventArgs
{
    public CreateMapEventArgs(Map map)
    {
        Map = map;
    }

    public Map Map;
}

public delegate void CreateGameEventHandler(object sender, CreateGameEventArgs e);
public delegate void CreateMapEventHandler(object sender, CreateMapEventArgs e);

public class SceneEvents : MonoBehaviour
{
    public static event EventHandler StartingNewGame;
    public static event EventHandler Terminating;
    public static event CreateGameEventHandler CreatingGame;
    public static event CreateMapEventHandler CreatingMap;

    private static SceneEvents _instance;

    public static SceneEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneEvents>();
            }

            return _instance;
        }
    }

    public void StartNewGame()
    {
        if (StartingNewGame != null)
        {
            StartingNewGame(this, EventArgs.Empty);
        }
    }

    public void Terminate()
    {
        if (Terminating != null)
        {
            Terminating(this, EventArgs.Empty);
        }
        Application.Quit();
    }

    public void CreateGame(int mapWidth, int mapHeight)
    {
        if (CreatingGame != null)
        {
            CreatingGame(this, new CreateGameEventArgs(mapWidth, mapHeight));
        }
    }

    public void CreateMap(Map map)
    {
        if (CreatingMap != null)
        {
            CreatingMap(this, new CreateMapEventArgs(map));
        }
    }
}
