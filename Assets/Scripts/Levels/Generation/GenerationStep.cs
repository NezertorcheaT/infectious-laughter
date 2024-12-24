using UnityEngine;

namespace Levels.Generation
{
    [RequireComponent(typeof(LevelGeneration))]
    public abstract class GenerationStep : MonoBehaviour
    {
        public abstract void Execute(LevelGeneration.Properties properties);
    }

    public abstract class TilemapStep : GenerationStep
    {
    }
}