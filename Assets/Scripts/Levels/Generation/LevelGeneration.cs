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
            [Tooltip("Ну это как бы сид, задаётся в сохранениях")] [SerializeField]
            public string Seed;
            
            [Tooltip("Это куда собственно тайлы записываться будут")] [SerializeField]
            public Tilemap Tilemap;

            public List<NonTileObject> NonTileObjects = new();
            public Random Random;
            [HideInInspector] public int LayerMinX;
            [HideInInspector] public int LayerMaxX;
            [HideInInspector] public int MaxY = 10;
            [HideInInspector] public int StructureMinX;
            [HideInInspector] public int StructureMaxX;

            public struct NonTileObject : IEquatable<NonTileObject>
            {
                public GameObject Prefab;
                public Vector3 Position;
                public Quaternion Rotation;
                public float OffsetY;
                public float OffsetX;

                public bool Equals(NonTileObject other) => Equals(Prefab, other.Prefab);
                public override bool Equals(object obj) => obj is NonTileObject other && Equals(other);

                public override int GetHashCode() => Prefab != null ? Prefab.GetHashCode() : 0;

                public static bool operator ==(NonTileObject left, NonTileObject right) => left.Equals(right);
                public static bool operator !=(NonTileObject left, NonTileObject right) => !left.Equals(right);
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


        public void InstantiateNonTile(Func<GameObject, Vector3, Quaternion, Transform, GameObject> instantiate)
        {
            (Vector3 pos, Properties.NonTileObject nonTile)[] positions = properties.NonTileObjects
                .Select(nonTile =>
                    (
                        nonTile.OffsetY == 0
                            ? nonTile.Position
                            : new Vector3(
                                nonTile.Position.x + nonTile.OffsetX,
                                Physics2D.Raycast(
                                    new Vector2(nonTile.Position.x + nonTile.OffsetX, properties.MaxY * 2f),
                                    Vector2.down,
                                    properties.MaxY * 10,
                                    1 << 0
                                ).point.y + nonTile.OffsetY,
                                nonTile.Position.z
                            ),
                        preSpawned: nonTile
                    )
                )
                .ToArray();

            foreach (var (pos, nonTile) in positions)
            {
                var i = instantiate(
                    nonTile.Prefab,
                    pos,
                    nonTile.Rotation,
                    null
                );

                i.SetActive(true);
            }
        }
    }
}