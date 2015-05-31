﻿
namespace View.Map
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Model.Map;

    public delegate void MapLayerChunkClickHandler(Vector3 worldPosition);

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(MeshFilter))]
    public class MapChunk : MonoBehaviour, IPointerClickHandler
    {
        public const int BlockSize = 10;

        public event MapLayerChunkClickHandler Clicked;

        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private ThreadedGenerator _job;

        private class ThreadedGenerator: ThreadedJob
        {
            private Map _map;
            private int _startColumn;
            private int _startRow;
            private TerrainTextureDefinition _ttd;
            private TerrainDefinition _td;
            private MapChunkGenerator _generator;

            public ThreadedGenerator(Map map, int startColumn, int startRow, TerrainTextureDefinition ttd, TerrainDefinition td)
            {
                _map = map;
                _startColumn = startColumn;
                _startRow = startRow;
                _ttd = ttd;
                _td = td;
                _generator = new MapChunkGenerator();
            }

            protected override void ThreadFunction()
            {
                _generator.Generate(_map, _startColumn, _startRow, _td.Terrain, _ttd);
            }

            public void PopulateMesh(MeshRenderer mr, MeshFilter mf)
            {
                _generator.PopulateMesh(mr, mf);
            }
        }

        public void Populate(Map map, int startColumn, int startRow, TerrainTextureDefinition ttd)
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
                _meshFilter = GetComponent<MeshFilter>();
            }

            _job = new ThreadedGenerator(map, startColumn, startRow, ttd, Game.Instance.Terrain);
            _job.Start();
        }
        
        void Update()
        {
            if ((_job != null) && (_job.IsDone))
            {
                _job.PopulateMesh(_meshRenderer, _meshFilter);
                _job = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerId == PointerInputModule.kMouseLeftId)
            {
                if (Clicked != null)
                {
                    Clicked(eventData.worldPosition);
                }
            }
        }
    }
}

