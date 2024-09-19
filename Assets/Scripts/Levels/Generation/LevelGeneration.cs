using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Levels.Generation
{
    [AddComponentMenu("Tilemap/Level Generation", 0)]
    public class LevelGeneration : MonoBehaviour
    {
        [Serializable]
        public class Properties
        {
            [Tooltip("Ну это как бы сид, задаётся в сохранениях")]
            [SerializeField] public string Seed;

            [Tooltip("Этот тайлик будет заменён на нихуя при генерации структур")]
            [SerializeField] public TileBase VoidTile;

            [Tooltip("Это куда собственно тайлы записываться будут")]
            [SerializeField] public Tilemap Tilemap;

            public List<PreSpawned> PreSpawns = new();
            public Random Random;
            [HideInInspector] public int LayerMinX;
            [HideInInspector] public int LayerMaxX;
            [HideInInspector] public int MaxY = 10;
            [HideInInspector] public int StructureMinX;
            [HideInInspector] public int StructureMaxX;

            public struct PreSpawned
            {
                public GameObject Prefab;
                public Vector3 Position;
                public Quaternion Rotation;
                public float OffsetY;
                public float OffsetX;
            }
        }

        [SerializeField] private GenerationStep[] steps;
        [SerializeField] private Properties properties;
        private CompositeCollider2D _composite;
        private TilemapCollider2D _tilemapCollider;

        public void SetSeed(string seed)
        {
            properties.Seed = seed;
        }

        public void StartGeneration()
        {
            _composite = properties.Tilemap.GetComponent<CompositeCollider2D>();
            _tilemapCollider = properties.Tilemap.GetComponent<TilemapCollider2D>();
            properties.Random = new Random(properties.Seed.GetHashCode());

            foreach (var step in steps)
            {
                step.Execute(properties);
                if (step is TilemapStep)
                    ProcessColliders();
            }
        }

        private void ProcessColliders()
        {
            _tilemapCollider?.ProcessTilemapChanges();
            _composite?.GenerateGeometry();
        }


        public void InstantiatePreSpawned(Func<GameObject, Vector3, Quaternion, Transform, GameObject> instantiate)
        {
            (Vector3 pos, Properties.PreSpawned preSpawned)[] positions = properties.PreSpawns
                .Select(preSpawned =>
                    (
                        preSpawned.OffsetY == 0
                            ? preSpawned.Position
                            : new Vector3(
                                preSpawned.Position.x + preSpawned.OffsetX,
                                Physics2D.Raycast(
                                    new Vector2(preSpawned.Position.x + preSpawned.OffsetX, properties.MaxY),
                                    Vector2.down,
                                    properties.MaxY * 5,
                                    1 << 0
                                ).point.y + preSpawned.OffsetY,
                                preSpawned.Position.z
                            ),
                        preSpawned
                    )
                )
                .ToArray();

            foreach (var (pos, preSpawned) in positions)
            {
                var i = instantiate(
                    preSpawned.Prefab,
                    pos,
                    preSpawned.Rotation,
                    null
                );

                i.SetActive(true);
            }
        }
    }
}