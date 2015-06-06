
namespace View.Map
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Model.Map;

    internal class MapChunkGenerator
    {
        public static int BLOCK_SIZE = 10;

        // Local caching to prevent creating these over and over
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector3> _normals;
        private List<Vector2> _uv;
        private List<Color32> _colours;
        private Vector3 _normal;

        private TerrainTextureDefinition _ttd;

        public MapChunkGenerator()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _normals = new List<Vector3>();
            _uv = new List<Vector2>();
            _colours = new List<Color32>();
            _normal = new Vector3(0, 0, -1);
        }

        public void Generate(Map map, int startColumn, int startRow, ICollection<MapTerrain> terrains, TerrainTextureDefinition ttd)
        {
            _vertices.Clear();
            _triangles.Clear();
            _normals.Clear();
            _uv.Clear();
            _colours.Clear();
            _ttd = ttd;

            GenerateTiles(map, startColumn, startRow, terrains, ttd);
        }

        public void PopulateMesh(MeshRenderer mr, MeshFilter meshFilter)
        {
            mr.sharedMaterial = _ttd.Material;

            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.colors32 = _colours.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.normals = _normals.ToArray();
            mesh.uv = _uv.ToArray();

            GameObject.Destroy(meshFilter.sharedMesh);
            meshFilter.mesh = mesh;
        }

        private void GenerateTiles(Map map, int startColumn, int startRow, ICollection<MapTerrain> terrains, TerrainTextureDefinition ttd)
        {
            for (int r = 0; r < BLOCK_SIZE; r++)
            {
                for (int c = 0; c < BLOCK_SIZE; c++)
                {
                    int x = c + startColumn;
                    int y = r + startRow;

                    if ((x >= 0) && (x < map.Width) && (y >= 0) && (y < map.Height))
                    {
                        MapTile mapTile = map.Tile[x, y];

                        if (mapTile != null)
                        {
                            Rect uvc = ttd.ByTerrain(mapTile.Terrain).Floor;
                            GenerateTile(c, r, y, ref uvc);

                            foreach (MapTerrain terrain in terrains)
                            {
                                TileCompass fringe = mapTile.GetFringe(terrain);
                                if (fringe != TileCompass.None)
                                {
                                    TerrainTileDefinition target = ttd.ByTerrain(terrain);
                                    if (target == null)
                                    {
                                        throw new InvalidOperationException(string.Format("Unable to locate terrain '{0}'", terrain.Name));
                                    }

                                    if (target.Fringe.ContainsKey((int)fringe))
                                    {
                                        uvc = target.Fringe[(int)fringe];
                                        GenerateTile(c, r, y, ref uvc);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException(String.Format("Unable to locate edge '{0}'", fringe));
                                    }
                                }
                            }

                            if (mapTile.IsWall)
                            {
                                TileCompass walls = mapTile.GetWalls(mapTile.Terrain);
                                TerrainTileDefinition target = ttd.ByTerrain(mapTile.Terrain);
                                if (target == null)
                                {
                                    throw new InvalidOperationException(string.Format("Unable to locate terrain '{0}'", mapTile.Terrain.Name));
                                }

                                if (target.Walls.ContainsKey((int)walls))
                                {
                                    uvc = target.Walls[(int)walls];
                                    GenerateTile(c, r, y, ref uvc);
                                }
                                else
                                {
                                    Debug.LogWarning(String.Format("Unable to locate wall '{0}' ({1}) for '{2}'", walls, (int)walls, mapTile.Terrain.Name));
                                    //throw new InvalidOperationException(String.Format("Unable to locate wall '{0}'", walls));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GenerateTile(int c, int r, int z, ref Rect uvc)
        {
            int vertexIndex = _vertices.Count;
            _vertices.Add(new Vector3(c + -0.5f, r + -0.5f, z));
            _vertices.Add(new Vector3(c + 0.5f, r + -0.5f, z));
            _vertices.Add(new Vector3(c + 0.5f, r + 0.5f, z));
            _vertices.Add(new Vector3(c + -0.5f, r + 0.5f, z));

            _triangles.Add(vertexIndex + 3);
            _triangles.Add(vertexIndex + 2);
            _triangles.Add(vertexIndex + 1);
            _triangles.Add(vertexIndex + 1);
            _triangles.Add(vertexIndex + 0);
            _triangles.Add(vertexIndex + 3);

            _normals.Add(_normal);
            _normals.Add(_normal);
            _normals.Add(_normal);
            _normals.Add(_normal);

            _uv.Add(new Vector2(uvc.x, uvc.y));
            _uv.Add(new Vector2(uvc.width, uvc.y));
            _uv.Add(new Vector2(uvc.width, uvc.height));
            _uv.Add(new Vector2(uvc.x, uvc.height));

            _colours.Add(Color.white);
            _colours.Add(Color.white);
            _colours.Add(Color.white);
            _colours.Add(Color.white);
        }
    }
}
