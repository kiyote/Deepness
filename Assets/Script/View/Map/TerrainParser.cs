
namespace View.Map
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Model.Map;

    public class TerrainParser : MonoBehaviour
    {

        private TerrainTextureDefinition _definition;
        private int _tileSize;

        public Material TerrainMaterial;

        // Use this for initialization
        void Start()
        {
            SystemEvents.InitializingSystems += InitializingSystems;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitializingSystems(object sender, EventArgs e)
        {
            _definition = new TerrainTextureDefinition(TerrainMaterial);

            Texture t = (Texture)Resources.Load(@"Texture/terrain", typeof(Texture2D));
            TerrainMaterial.mainTexture = t;

            LoadTerrains();
            LoadTerrainDefinition(t.width, t.height);
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

                    Game.Instance.Terrain.Add(new MapTerrain(id, name, blocking));
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
                if ((parts.Length > 0) && (parts[0] != string.Empty))
                {
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

                    MapTerrain t = Game.Instance.Terrain.ByName(terrain);
                    TerrainTileDefinition tileDefn = _definition.ByTerrain(t);
                    if (tileDefn == null)
                    {
                        tileDefn = _definition.Create(t);
                    }

                    Rect uv = new Rect(left / textureWidth, (textureHeight - (top + height)) / textureHeight, (left + width) / textureWidth, (textureHeight - top) / textureHeight);
                    if (string.Compare(type, "floor", System.StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (_tileSize == 0)
                        {
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

            SystemEvents.Instance.TerrainParse(_definition);
        }
    }
}
