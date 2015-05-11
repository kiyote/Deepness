
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

            System.Random r = new System.Random();
            int count = (int)((float)(width * height) * 0.5f);
            for (int i = 0; i < count; i++)
            {
                //_map.Tile[r.Next(_map.Width), r.Next(_map.Height)].Terrain.Floor = _terrain["grass"];
                map.Tile[r.Next(width), r.Next(height)].IsWall = true;
            }

            map.EndCreate();
        }
    }
}

