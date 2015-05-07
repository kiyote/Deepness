using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainTileDefinition
{
	public Rect Floor;
	public Dictionary<int, Rect> Fringe;
}

public class TerrainParser : MonoBehaviour {

	private Dictionary<Terrain, TerrainTileDefinition> _terrain;
	private int _tileSize;

    private Dictionary<string, Terrain> _definitions;

	public Material TerrainMaterial;

	public Dictionary<Terrain, TerrainTileDefinition> Definition
	{
		get
		{
            return _terrain;
		}
	}

    public Dictionary<string, Terrain> Terrain
    {
        get
        {
            return _definitions;
        }
    }

	public int TileSize
	{
		get
		{
			return _tileSize;
		}
	}

	// Use this for initialization
	void Start () {
		_terrain = new Dictionary<Terrain, TerrainTileDefinition>();
        _definitions = new Dictionary<string, Terrain>();

		Texture t = (Texture)Resources.Load(@"Texture/terrain", typeof(Texture2D));
		TerrainMaterial.mainTexture = t;

        LoadTerrains();
		LoadTerrainDefinition(t.width, t.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadTerrains()
    {
        TextAsset defn = (TextAsset)Resources.Load(@"terrain_definition", typeof(TextAsset));
        string[] lines = defn.text.Split('\n');
        foreach (string s in lines)
        {
            string line = s.Trim();
            if (!line.StartsWith("#"))
            {
                string[] parts = line.Split('=');

                int id = int.Parse(parts[0]);

                string[] values = parts[1].Split(',');
                string name = values[0];
                bool blocking = bool.Parse(values[1]);

                _definitions[name] = new Terrain(id, name, blocking);
            }
        }
    }

	private void LoadTerrainDefinition(float textureWidth, float textureHeight)
	{
		TextAsset defn = (TextAsset)Resources.Load(@"Texture/terrain", typeof(TextAsset));
		string[] lines = defn.text.Split('\n');
		float halfXPixel = 0.5f / textureWidth;
		float halfYPixel = 0.5f / textureHeight;
		foreach (string s in lines)
		{
			string[] parts = s.Split('=');
			if ((parts.Length > 0) && (parts[0] != string.Empty)) {
				string[] lookups = parts[0].Trim().Split('_');
				string terrain = lookups[0];
				string type = lookups[1];
				int value = -1;
				if (lookups.Length == 3)
				{
					value = int.Parse(lookups[2]);
				}

				string[] coords = parts[1].Trim().Split(' ');
				float left = float.Parse(coords[0]) + halfXPixel;
				float top = float.Parse(coords[1]) + halfYPixel;
				float width = float.Parse(coords[2]);
				float height = float.Parse(coords[3]);

                Terrain t = _definitions[terrain];
				TerrainTileDefinition tileDefn;
				if (_terrain.ContainsKey(t) == false)
				{
					tileDefn = new TerrainTileDefinition();
					tileDefn.Fringe = new Dictionary<int, Rect>();
					_terrain[t] = tileDefn;
				}
				tileDefn = _terrain[t];

				Rect uv = new Rect(left / textureWidth, (textureHeight - (top + height)) / textureHeight, (left + width) / textureWidth, (textureHeight - top) / textureHeight);
				if (string.Compare(type, "floor", System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (_tileSize == 0) {
						_tileSize = (int)width;
					}

					tileDefn.Floor = uv;
				}
				else if (string.Compare(type, "fringe", System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					tileDefn.Fringe[value] = uv;
				}
			}
		}
	}
}
