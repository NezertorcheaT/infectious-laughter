using JetBrains.Annotations;
using UnityEngine;

namespace Entity.AI
{
    /// <summary>
    /// управляющая часть мозга<br/>
    /// работает как абилити, но должно висеть на мозге<br/>
    /// пж не двигайте сущности в нейронах, это управляющая конструкция, через нё ведётся управение способностями, выключая, включая, и используя методы способностей
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

        /// <summary>
        /// бесполезная хуйня, аналог isActiveAndEnabled
        /// </summary>
        /// <returns></returns>
        public virtual bool Available() => isActiveAndEnabled && _initialized;

        /// <summary>
        /// аналог старта, чтоб была полная инициализация
        /// </summary>
        public abstract void AfterInitialize();
    }
}