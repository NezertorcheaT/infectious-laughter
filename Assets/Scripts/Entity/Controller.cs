using UnityEngine;

namespace Entity
{
    public abstract class Controller : MonoBehaviour
    {
        public global::Entity.Entity Entity {  get; private set; }

        public virtual void Initialize() { Entity = GetComponent<global::Entity.Entity>(); }
    }
}