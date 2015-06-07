
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
                    Tile tile = map.Tile[c, r];
                    tile.Terrain = terrains[0];
                    //tile.IsWall = terrains[0].Walls;
                }
            }

            int count = (int)((float)(width * height) * 0.5f);
            for (int i = 0; i < count; i++)
            {
                Tile tile = map.Tile[rand.Next(width), rand.Next(height)];
                MapTerrain terrain = terrains[rand.Next(terrains.Count)];
                tile.Terrain = terrain;
                //tile.IsWall = terrain.Walls;
            }

            map.EndCreate();
        }
    }
}

