using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Generation
{
    public abstract class GroundOffsetLayer : ScriptableObject
    {
        [field: Tooltip("тайлик, который будет прибавляться к земле")]
        [field: SerializeField] public TileBase Tile { get; private set; }
        
        /// <summary>
        /// для получения последовательности высот
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public abstract IEnumerable<float> GetMap(string seed);
        
        public abstract bool Infinite { get; }
    }
}