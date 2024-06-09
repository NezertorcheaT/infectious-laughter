using UnityEngine;

namespace Inventory.Garbage
{
    public class PointTargetForGarbageAnimation
    {
        public Transform Target { get; private set; }
        public PointTargetForGarbageAnimation(Transform target) => Target = target;
    }
}