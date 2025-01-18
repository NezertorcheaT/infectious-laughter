using CustomHelper;
using JetBrains.Annotations;
using UnityEngine;

namespace Entity.AI
{
    /// <summary>
    /// управляющая часть мозга<br/>
    /// работает как абилити, но должно висеть на мозге<br/>
    /// пж не двигайте сущности в нейронах, это управляющая конструкция, через неё ведётся управение способностями, выключая, включая, и используя методы способностей
    /// </summary>
    public abstract class Neurone : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        public Brain Brain { get; private set; }

        private bool _initialized;

        /// <summary>
        /// внутренний метод для инициализации нейрона
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="brain"></param>
        public void Initialize([NotNull] Entity entity, [NotNull] Brain brain)
        {
            if (_initialized) return;
            Entity = entity;
            Brain = brain;
            _initialized = true;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// важно!!!<br/>
        /// это не обычный трансформ, как и в абилити он кеширован<br/>
        /// пока не произошла инициализация, он будет возвращать оригинальный трансформ этого нейрона<br/>
        /// после инициализации возвращает трансформ сущности
        /// </summary>
        public new Transform transform => _initialized ? Entity.transform : base.transform;

        /// <summary>
        /// бесполезная хуйня, аналог isActiveAndEnabled, око круче
        /// </summary>
        /// <returns></returns>
        public virtual bool Available() => isActiveAndEnabled && _initialized && !gameObject.IsPrefab();

        /// <summary>
        /// аналог старта, чтоб была полная инициализация
        /// </summary>
        public abstract void AfterInitialize();
    }
}