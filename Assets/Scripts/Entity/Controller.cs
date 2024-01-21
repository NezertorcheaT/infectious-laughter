using UnityEngine;

namespace Scripts.Entity
{
    public abstract class Controller : MonoBehaviour
    {
        public Entity Entity {  get; private set; }

        public virtual void Initialize() { Entity = GetComponent<Entity>(); }
    }
}