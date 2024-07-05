using System;
using UnityEngine;

namespace Entity.States.StateObjects.Edits
{
    [Serializable]
    [CreateAssetMenu(fileName = "Wait Edit", menuName = "AI Nodes/Edits/Wait Edit", order = 0)]
    public class WaitStateEdit : EditableStateProperties
    {
        [Min(0)] public float time = 2f;

        public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
            ? (T) GetType().GetField(name).GetValue(this)
            : default;

        public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
    }
}