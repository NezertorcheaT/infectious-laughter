using System;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// ну значт контроллер<br />
    /// вешается рядом с ентити и только один<br />
    /// нужен чтоб контролировать поведение сущности
    /// </summary>
    public abstract class Controller : MonoBehaviour, IInitializeByEntity
    {
        /// <summary>
        /// ссылочка на контролируемую сущность
        /// </summary>
        public Entity Entity { get; private set; }

        /// <summary>
        /// готово ли к запуску
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// когда инициализация кончена
        /// </summary>
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

    /// <summary>
    /// штука, которая может быть инициализирована от сущности<br />
    /// обычно это контроллеры и способности
    /// </summary>
    public interface IInitializeByEntity
    {
        /// <summary>
        /// сущность, к которой относится компонент
        /// </summary>
        Entity Entity { get; }

        /// <summary>
        /// это нужно для инициализации типа вместо старта<br />
        /// всегда юзать <code>base.Initialize()</code> вначале, иначе ты пидр
        /// </summary>
        void Initialize();

        /// <summary>
        /// готово ли к запуску
        /// </summary>
        bool Initialized { get; set; }
    }
}