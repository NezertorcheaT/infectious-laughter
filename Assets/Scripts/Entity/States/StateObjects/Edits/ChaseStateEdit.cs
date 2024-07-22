using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.States.StateObjects.Edits
{
    [Serializable]
    [CreateAssetMenu(fileName = "Chase Edit", menuName = "AI Nodes/Edits/Chase Edit", order = 0)]
    public class ChaseStateEdit : EditableStateProperties
    {
        [Min(0.001f)] public float rayDistance;
        public LayerMask groundLayer;
        public bool initialDirection;
        [Min(0)] public int next;
        public float visionDistance; // ���������� ������
        public LayerMask playerLayer; // ���� ������

        private void Reset()
        {
            groundLayer = LayerMask.GetMask("Default");
            rayDistance = 0.1f;
            visionDistance = 10f; // �� ��������� 10 ������
        }

        public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
            ? (T)GetType().GetField(name).GetValue(this)
            : default;

        public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
    }
}
