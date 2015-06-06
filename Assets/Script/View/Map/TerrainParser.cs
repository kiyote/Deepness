
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
            MessageBus.Get().Subscribe<InitializeSystemEvent>(InitializingSystems);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitializingSystems(object sender, InitializeSystemEvent e)
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
            SpriteSheetParser parser = new SpriteSheetParser();
            IEnumerable<SpriteSheetParser.Entry> entries = parser.Parse(defn, textureWidth, textureHeight);

            foreach (SpriteSheetParser.Entry entry in entries)
            {
                MapTerrain t = Game.Instance.Terrain.ByName(entry.Terrain);
                TerrainTileDefinition tileDefn = _definition.ByTerrain(t);
                if (tileDefn == null)
                {
                    tileDefn = _definition.Create(t);
                }

                if (string.Compare(entry.Type, "floor", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (_tileSize == 0)
                    {
                        _tileSize = 24;
                    }

                    tileDefn.Floor = entry.Coordinates;
                }
                else if (string.Compare(entry.Type, "edge", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    tileDefn.Fringe[entry.Value] = entry.Coordinates;
                }
                else if (string.Compare(entry.Type, "wall", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    tileDefn.Walls[entry.Value] = entry.Coordinates;
                }
            }

            MessageBus.Get().Publish<TerrainParsedEvent>(this, new TerrainParsedEvent(_definition));
        }
    }
}
