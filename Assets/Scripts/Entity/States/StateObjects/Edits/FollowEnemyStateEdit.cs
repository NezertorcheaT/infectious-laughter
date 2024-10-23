using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.States.StateObjects.Edits
{
    [Serializable]
    [CreateAssetMenu(fileName = "FollowEnemy Edit", menuName = "AI Nodes/Edits/FollowEnemy Edit", order = 0)]
    public class FollowEnemyStateEdit : EditableStateProperties
    {
        public bool initialDirection;
        [Min(0)] public int next;

        public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
            ? (T)GetType().GetField(name).GetValue(this)
            : default;

        public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
    }
}
