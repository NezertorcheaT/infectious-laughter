using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Levels.Generation.OffsetLayers
{
    [CreateAssetMenu(fileName = "New Random Layer", menuName = "Generation Layers/Random", order = 0)]
    public class RandomOffsetLayer : GroundOffsetLayer
    {
        [SerializeField] private int min;
        [SerializeField] private int max;
        [SerializeField, Min(1)] private int size = 1;

        public override IEnumerable<float> GetMap(string seed)
        {
            var random = new Random(seed.GetHashCode());
            while (true)
            {
                var value = random.Next(min, max + 1);
                for (var i = 0; i < size; i++)
                {
                    yield return value;
                }
            }
        }

        public override bool Infinite => true;
    }
}