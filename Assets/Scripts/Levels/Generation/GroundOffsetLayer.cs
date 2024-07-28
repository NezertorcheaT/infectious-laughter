using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Generation
{
    public abstract class GroundOffsetLayer : ScriptableObject
    {
        [field: SerializeField] public TileBase Tile { get; private set; }
        
        public abstract IEnumerable<float> GetMap(string seed);
        public abstract bool Infinite { get; }
    }
}