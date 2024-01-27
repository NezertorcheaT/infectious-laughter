using UnityEngine;

namespace Entity
{
    public abstract class Ability : MonoBehaviour
    {
        public global::Entity.Entity Entity { get; private set; }

        // Нужно для активации галочки выключения скрипта в инспекторе
        protected virtual void Start() { }

        public virtual void Initialize() => Entity = GetComponent<global::Entity.Entity>();

        public virtual bool Available() => isActiveAndEnabled;
    }
}