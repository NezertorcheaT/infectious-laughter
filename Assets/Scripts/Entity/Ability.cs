using UnityEngine;

namespace Scripts.Entity
{
    public abstract class Ability : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        
        protected virtual void Start() { } // Нужно для активации галочки выключения скрипта в инспекторе
        public virtual void Initialize() { Entity = GetComponent<Entity>(); }
        public virtual bool Available() { return isActiveAndEnabled; }
    }
}