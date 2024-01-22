using UnityEngine;

namespace Scripts.Entity
{
    public abstract class Ability : MonoBehaviour
    {
        public Entity Entity { get; private set; }

        protected virtual void Start() { } // ����� ��� ��������� ������� ���������� ������� � ����������
        public virtual void Initialize() { Entity = GetComponent<Entity>(); }
        public virtual bool Avaiable() { return isActiveAndEnabled; }
    }
}