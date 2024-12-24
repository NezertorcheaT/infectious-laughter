using UnityEngine;

namespace Entity
{
    /// <summary>
    /// ну значт способность<br />
    /// отвечают за выполнение действия у сущностей
    /// </summary>
    public abstract class Ability : MonoBehaviour, IInitializeByEntity
    {
        /// <summary>
        /// ссылочка на контролируемую сущность
        /// </summary>
        public Entity Entity { get; private set; }

        public virtual void Initialize() => Entity = GetComponent<Entity>();

        /// <summary>
        /// бесполезная хуйня, аналог isActiveAndEnabled
        /// </summary>
        /// <returns></returns>
        public virtual bool Available() => isActiveAndEnabled;

        bool IInitializeByEntity.Initialized
        {
            get => IsInitialized;
            set => IsInitialized = value;
        }

        /// <summary>
        /// готово ли к запуску
        /// </summary>
        public bool IsInitialized { get; private set; }
    }
}