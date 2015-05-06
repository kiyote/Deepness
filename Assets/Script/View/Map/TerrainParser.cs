using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainTileDefinition
{
	public Rect Floor;
	public Dictionary<int, Rect> Wall;
}

public class TerrainParser : MonoBehaviour {

	private Dictionary<string, TerrainTileDefinition> _terrain;
	private int _tileSize;

	public Material TerrainMaterial;

	public Dictionary<string, TerrainTileDefinition> Definition
	{
		get
		{
			return _terrain;
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
		_terrain = new Dictionary<string, TerrainTileDefinition>();

		Texture t = (Texture)Resources.Load(@"Texture/terrain", typeof(Texture2D));
		TerrainMaterial.mainTexture = t;

		LoadTerrainDefinition(t.width, t.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadTerrainDefinition(float textureWidth, float textureHeight)
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

				TerrainTileDefinition tileDefn;
				if (_terrain.ContainsKey(terrain) == false)
				{
					tileDefn = new TerrainTileDefinition();
					tileDefn.Wall = new Dictionary<int, Rect>();
					_terrain[terrain] = tileDefn;
				}
				tileDefn = _terrain[terrain];

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
					tileDefn.Wall[value] = uv;
				}
			}
		}
	}
}

/*
			string ts = s.Trim();
			if (ts.StartsWith("<sprite")) 
			{
				string[] parts = ts.Split(' ');

				string terrain = string.Empty;
				string type = string.Empty;
				int value = 0;
				float left = 0.0f;
				float top = 0.0f;
				float width = 0.0f;
				float height = 0.0f;

				foreach (string part in parts)
				{

					if (part.StartsWith("n"))
					{
						string name = part.Substring(part.IndexOf("\"")+1);
						name = name.Substring(0, name.IndexOf('.'));
						string[] pieces = name.Split('_');
						terrain = pieces[0];
						type = pieces[1];
						if (pieces.Length == 3)
						{
							value = int.Parse(pieces[2]);
						}
					} 
					else if (part.StartsWith("x"))
					{
						int start = part.IndexOf("\"")+1;
						int end = part.LastIndexOf("\"");
						string coord = part.Substring(start, end - start);
						left = float.Parse(coord);
					}
					else if (part.StartsWith("y"))
					{
						int start = part.IndexOf("\"")+1;
						int end = part.LastIndexOf("\"");
						string coord = part.Substring(start, end - start);
						top = float.Parse(coord);
					}
					else if (part.StartsWith("w"))
					{
						int start = part.IndexOf("\"")+1;
						int end = part.LastIndexOf("\"");
						string coord = part.Substring(start, end - start);
						width = float.Parse(coord);
					}
					else if (part.StartsWith("h"))
					{
						int start = part.IndexOf("\"")+1;
						int end = part.LastIndexOf("\"");
						string coord = part.Substring(start, end - start);
						height = float.Parse(coord);
					}
				}
				TerrainTileDefinition tileDefn;
				if (_terrain.ContainsKey(terrain) == false)
				{
					tileDefn = new TerrainTileDefinition();
					tileDefn.Wall = new Dictionary<int, Rect>();
					_terrain[terrain] = tileDefn;
				}
				tileDefn = _terrain[terrain];

				left += 1.0f;
				
				Rect uv = new Rect(left / textureWidth, (textureHeight - (top + height)) / textureHeight, (left + width) / textureWidth, (textureHeight - top) / textureHeight);
				if (string.Compare(type, "floor", System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (_tileSize == 0) {
						_tileSize = (int)width;
					}
					
					tileDefn.Floor = uv;
				}
				else if (string.Compare(type, "wall", System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					tileDefn.Wall[value] = uv;
				}

			}
 */