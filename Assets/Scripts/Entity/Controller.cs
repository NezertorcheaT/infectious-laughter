using System;
using UnityEngine;

namespace Entity
{
    public abstract class Controller : MonoBehaviour, IInitializeByEntity
    {
        public Entity Entity { get; private set; }
        public bool IsInitialized { get; private set; }
        public Action OnInitializationComplete;
        
        public virtual void Initialize()
        {
            Entity = GetComponent<Entity>();
        }

        bool IInitializeByEntity.Initialized
        {
            get => IsInitialized;
            set => IsInitialized = value;
        }
    }

    public interface IInitializeByEntity
    {
        void Initialize();
        bool Initialized { get; set; }
    }
}