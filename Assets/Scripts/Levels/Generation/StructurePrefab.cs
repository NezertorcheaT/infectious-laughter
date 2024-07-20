using UnityEngine;

namespace Levels.Generation
{
    public class StructurePrefab : GenerationPrefab
    {
        [field: SerializeField, Min(1)] public int Count { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int Width { get; private set; } = 1;
    }
}