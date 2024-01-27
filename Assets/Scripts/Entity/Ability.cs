using UnityEngine;

namespace Scripts.Entity
{
    public abstract class Ability : MonoBehaviour
    {
        public Entity Entity { get; private set; }

        // Нужно для активации галочки выключения скрипта в инспекторе
        protected virtual void Start() { }

        public virtual void Initialize() => Entity = GetComponent<Entity>();

        public virtual bool Available() => isActiveAndEnabled;
    }
}