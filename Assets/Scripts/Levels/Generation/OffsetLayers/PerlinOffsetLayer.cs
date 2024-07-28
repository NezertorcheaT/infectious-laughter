using System.Collections.Generic;
using UnityEngine;

namespace Levels.Generation.OffsetLayers
{
    [CreateAssetMenu(fileName = "New Perlin Layer", menuName = "Generation Layers/Perlin", order = 0)]
    public class PerlinOffsetLayer : GroundOffsetLayer
    {
        [SerializeField] private float precision;
        [SerializeField, Min(0)] private float size = 1;
        [SerializeField, Min(1)] private int layers = 1;

        public override IEnumerable<float> GetMap(string seed)
        {
            //жаль, что не все поймут, в чем же дело
            var s = (float) seed.GetHashCode() / 9999999;
            while (true)
            {
                s += precision;

                float noise = 1;
                for (var i = 1; i < layers + 1; i++)
                {
                    noise *= Mathf.PerlinNoise(s * i, s * i);
                }

                yield return noise * size;
            }
        }

        public override bool Infinite => true;
    }
}