using UnityEngine;
using System;
using System.Collections.Generic;
using Model.Map;
using View.Map;

public class TerrainParsedEventArgs: EventArgs
{
    public TerrainParsedEventArgs(TerrainTextureDefinition terrainTextureDefinition)
    {
        TerrainTextureDefinition = terrainTextureDefinition;
    }

    public TerrainTextureDefinition TerrainTextureDefinition;
}

public class CameraUpdateEventArgs : EventArgs
{
    private readonly Rect _bounds;

    public CameraUpdateEventArgs(Rect bounds)
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

public class SystemEvents : MonoBehaviour
{
    public static event EventHandler<CameraUpdateEventArgs> CameraUpdated;
    public static event EventHandler<TerrainParsedEventArgs> TerrainParsed;
    public static event EventHandler InitializingSystems;

    private static SystemEvents _instance;

    public static SystemEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SystemEvents>();
            }

            return _instance;
        }
    }

    void Start()
    {
    }

    public void TerrainParse(TerrainTextureDefinition terrainTextureDefinition)
    {
        if (TerrainParsed != null)
        {
            TerrainParsed(this, new TerrainParsedEventArgs(terrainTextureDefinition));
        }
    }

    public void InitializeSystems()
    {
        if (InitializingSystems != null)
        {
            InitializingSystems(this, EventArgs.Empty);
        }
    }

    public void UpdateCamera(Rect bounds)
    {
        if (CameraUpdated != null)
        {
            CameraUpdated(this, new CameraUpdateEventArgs(bounds));
        }
    }

}
