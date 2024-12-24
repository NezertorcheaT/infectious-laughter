using System;
using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Offset Layers")]
    public class ApplyingOffsetLayers : TilemapStep
    {
        [Tooltip("Это кароч слои изменения террейна, они будут двигать террейн вверх и вниз, в зависимости от них самих")]
        [SerializeField] private OffsetLayer[] layers;

        [Serializable]
        private class OffsetLayer
        {
            public enum Behavior
            {
                Clamp,
                Repeat,
                Mirror
            }

            public enum Blending
            {
                Add,
                Subtract
            }

            [Tooltip("слой смещения")] public GroundOffsetLayer layer;

            [Tooltip("добавлять или вычитать слой")]
            public Blending blending;

            [Tooltip("поведение типа обрезать в конце, повторять, отразить")]
            public Behavior behavior;

            [Tooltip("смещение типа вправо влево")]
            public int offset;
        }

        public override void Execute(LevelGeneration.Properties properties)
        {
            foreach (var offsetLayer in layers)
            {
                if (offsetLayer.layer.Infinite)
                {
                    ProcessLayer(offsetLayer.layer.GetMap(properties.Seed), offsetLayer, properties.LayerMinX,
                        properties.LayerMaxX, properties);
                    return;
                }

                var map = offsetLayer.layer.GetMap(properties.Seed).ToArray();
                offsetLayer.offset = (int) Mathf.Repeat(offsetLayer.offset, map.Length);

                if (offsetLayer.behavior is OffsetLayer.Behavior.Clamp)
                    ProcessLayer(map, offsetLayer, properties.LayerMaxX,
                        Mathf.Clamp(map.Length - 1, properties.LayerMinX, properties.LayerMaxX),
                        properties);
                else if (offsetLayer.behavior is OffsetLayer.Behavior.Repeat)
                {
                    for (var i = -1; i < properties.LayerMaxX / map.Length; i++)
                    {
                        ProcessLayer(map, offsetLayer,
                            Mathf.Clamp((map.Length - 1) * i, properties.LayerMinX, properties.LayerMaxX),
                            Mathf.Clamp((map.Length - 1) * (i + 1), properties.LayerMinX,
                                properties.LayerMaxX),
                            properties
                        );
                    }
                }
                else if (offsetLayer.behavior is OffsetLayer.Behavior.Mirror)
                {
                    for (var i = -1; i < properties.LayerMaxX / map.Length; i++)
                    {
                        map = map.Reverse().ToArray();
                        ProcessLayer(map, offsetLayer,
                            Mathf.Clamp((map.Length - 1) * i, properties.LayerMinX, properties.LayerMaxX),
                            Mathf.Clamp((map.Length - 1) * (i + 1), properties.LayerMinX,
                                properties.LayerMaxX),
                            properties
                        );
                    }
                }
            }
        }

        private void ProcessLayer(IEnumerable<float> map, OffsetLayer layer, int from, int to,
            LevelGeneration.Properties levelGeneration)
        {
            map = map.Select(i => i * (layer.blending is OffsetLayer.Blending.Subtract ? -1f : 1f));
            var enumerator = map.GetEnumerator();

            try
            {
                for (var i = 0; i < layer.offset; i++)
                {
                    enumerator.MoveNext();
                }

                var x = from;
                while (enumerator.MoveNext())
                {
                    if (x >= to) break;
                    var current = enumerator.Current;
                    if (current == 0) continue;

                    var hit = levelGeneration.Tilemap.GridRay(new Vector2Int(x, levelGeneration.MaxY), Vector2.down);
                    if (!hit.isHit) continue;

                    if (current < 0)
                        levelGeneration.Tilemap.SetTile(new Vector3Int(x, hit.point.y), null);

                    for (var i = 0; i < Mathf.Abs(current) + 1; i++)
                    {
                        levelGeneration.Tilemap.SetTile(
                            new Vector3Int(
                                x,
                                hit.point.y + (current > 0 ? i : -i + 1)
                            ),
                            current > 0 ? layer.layer.Tile : null
                        );
                    }

                    x++;
                }
            }
            finally
            {
                enumerator?.Dispose();
            }
        }
    }
}