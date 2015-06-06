
using UnityEngine;
using System;
using View.Map;
using Model.Map;

#region System Events

public class InitializeSystemEvent : EventArgs
{
}

public class CameraUpdateEvent : EventArgs
{
    private readonly Rect _bounds;

    public CameraUpdateEvent(Rect bounds)
        : base()
    {
        _bounds = bounds;
    }

    public Rect Bounds
    {
        get
        {
            return _bounds;
        }
    }
}

public class TerrainParsedEvent : EventArgs
{
    public TerrainParsedEvent(TerrainTextureDefinition terrainTextureDefinition)
    {
        TerrainTextureDefinition = terrainTextureDefinition;
    }

    public TerrainTextureDefinition TerrainTextureDefinition;
}

#endregion

#region UI Events

public class MainMenuEvent : EventArgs
{
    private bool _show;

    public MainMenuEvent(bool show)
    {
        _show = show;
    }

    public bool Show
    {
        get
        {
            return _show;
        }
    }
}

public class NewGameMenuEvent : EventArgs
{
    private bool _show;

    public NewGameMenuEvent(bool show)
    {
        _show = show;
    }

    public bool Show
    {
        get
        {
            return _show;
        }
    }
}
#endregion

#region Game Events

public class CreateGameEvent : EventArgs
{
    private int _width;
    private int _height;

    public CreateGameEvent(int mapWidth, int mapHeight)
    {
        _width = mapWidth;
        _height = mapHeight;
    }

    public int MapWidth
    {
        get
        {
            return _width;
        }
    }

    public int MapHeight
    {
        get
        {
            return _height;
        }
    }
}

public class CreateMapEvent : EventArgs
{
    private Map _map;

    public CreateMapEvent(Map map)
    {
        _map = map;
    }

    public Map Map
    {
        get
        {
            return _map;
        }
    }
}

public class StartNewGameEvent : EventArgs
{
}

public class TerminateEvent : EventArgs
{
}

#endregion