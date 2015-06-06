
namespace Model.Map
{
    using System;
    using System.Collections.Generic;

    public class MapGenerator
    {
        public MapGenerator()
        {
        }

        public void Generate(Map map, int width, int height)
        {
            map.Create(width, height);
            map.BeginCreate();

            System.Random rand = new System.Random();

            List<MapTerrain> terrains = new List<MapTerrain>(Game.Instance.Terrain.Terrain);
            for (int r = 0; r < map.Height; r++)
            {
                for (int c = 0; c < map.Width; c++)
                {
                    map.Tile[c, r].Terrain = terrains[0];
                }
            }

            int count = (int)((float)(width * height) * 0.5f);
            for (int i = 0; i < count; i++)
            {
                //_map.Tile[r.Next(_map.Width), r.Next(_map.Height)].Terrain.Floor = _terrain["grass"];
                MapTile tile = map.Tile[rand.Next(width), rand.Next(height)];
                MapTerrain terrain = terrains[rand.Next(terrains.Count)];
                tile.Terrain = terrain;
                tile.IsWall = terrain.Walls;
            }

            map.EndCreate();
        }
    }
}

