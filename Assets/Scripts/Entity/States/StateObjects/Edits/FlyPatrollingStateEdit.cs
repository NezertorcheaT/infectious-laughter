using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.States.StateObjects.Edits
{
    [Serializable]
    [CreateAssetMenu(fileName = "FlyPatrollingState Edit", menuName = "AI Nodes/Edits/FlyPatrollingState Edit", order = 0)]
    public class FlyPatrollingStateEdit : EditableStateProperties
    {
        [Min(0.001f)] public float rayDistance;
        public LayerMask groundLayer;
        public bool initialDirection;
        public float maxFlightDistance;
        public float flightFrequency;
        public float flightAmplitude;
        [Min(0)] public int next;

        public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
            ? (T)GetType().GetField(name).GetValue(this)
            : default;

        public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
    }
}
