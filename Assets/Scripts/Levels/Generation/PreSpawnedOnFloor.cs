﻿using CustomHelper;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels.Generation
{
    public class PreSpawnedOnFloor : MonoBehaviour
    {
        [field: Tooltip("центер области пола")]
        [field: SerializeField]
        public Vector2 Center { get; private set; }

        [field: Tooltip("ширина области пола")]
        [field: SerializeField, Min(0)]
        public float Size { get; private set; }

        [field: Tooltip("колличество попыток поиска")]
        [field: SerializeField, Min(1)]
        public int MaxSearchTry { get; private set; } = 100;

        [field: Tooltip("минимальная единица поиска")]
        [field: SerializeField, Min(0.000001f)]
        public float SearchUnit { get; private set; } = 0.0625f;

        [field: Tooltip("размер коробок вниз, не влияет на конечный результат")]
        [field: FormerlySerializedAs("visualSizeY")]
        [field: SerializeField, Min(0)]
        public float RayStartHeight { get; private set; } = 4f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(
                Center.ToVector3() + transform.position + new Vector3(0, RayStartHeight / 2f),
                new Vector3(Size, RayStartHeight)
            );
            Gizmos.DrawWireCube(
                Center.ToVector3() + transform.position + new Vector3(0, RayStartHeight / 2f),
                new Vector3(Size + MaxSearchTry * SearchUnit, RayStartHeight)
            );
        }
    }
}