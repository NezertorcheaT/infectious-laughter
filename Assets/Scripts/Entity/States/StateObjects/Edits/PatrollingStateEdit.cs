using System;
using UnityEngine;

namespace Entity.States.StateObjects.Edits
{
    [Serializable]
    [CreateAssetMenu(fileName = "Patrolling Edit", menuName = "AI Nodes/Edits/Patrolling Edit", order = 0)]
    public class PatrollingStateEdit : EditableStateProperties
    {
        [Min(0.001f)] public float rayDistance;
        public LayerMask groundLayer;
        public bool initialDirection;
        [Min(0)] public int next;

        private void Reset()
        {
            groundLayer = LayerMask.GetMask("Default");
            rayDistance = 0.1f;
        }

        public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
            ? (T) GetType().GetField(name).GetValue(this)
            : default;

        public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
    }
}