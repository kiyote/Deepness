namespace View.Map
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Model.Map;

    public class TerrainTileDefinition
    {
        private Rect _floor;
        private Dictionary<int, Rect> _fringe;

        public TerrainTileDefinition()
        {
            _fringe = new Dictionary<int, Rect>();
        }

        public Rect Floor
        {
            get
            {
                return _floor;
            }
            set
            {
                _floor = value;
            }
        }

        public Dictionary<int, Rect> Fringe
        {
            get
            {
                return _fringe;
            }
        }
    }

    public class TerrainTextureDefinition
    {
        private Dictionary<MapTerrain, TerrainTileDefinition> _definitionByTerrain;
        private Material _material;

        public TerrainTextureDefinition(Material material)
        {
            _material = material;
            _definitionByTerrain = new Dictionary<MapTerrain, TerrainTileDefinition>();
        }

        public Material Material
        {
            get
            {
                return _material;
            }
        }

        public TerrainTileDefinition Create(MapTerrain terrain)
        {
            TerrainTileDefinition definition = new TerrainTileDefinition();
            _definitionByTerrain[terrain] = definition;

            return definition;
        }

        public TerrainTileDefinition ByTerrain(MapTerrain terrain)
        {
            if (_definitionByTerrain.ContainsKey(terrain))
            {
                return _definitionByTerrain[terrain];
            }
            return null;
        }
    }

}