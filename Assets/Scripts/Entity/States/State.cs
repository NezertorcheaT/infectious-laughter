using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [Serializable]
    public abstract class State : ScriptableObject
    {
        /// <summary>
        /// имя для редактора
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// не менять пж
        /// </summary>
        public abstract int Id { get; set; }

        /// <summary>
        /// асинхронный метод действия
        /// </summary>
        /// <param name="entity">над кем будет действие произведено</param>
        /// <param name="previous">предыдущее состояние, хз может нужно кому</param>
        /// <param name="properties">эта кароч типа аргуметы дерева</param>
        /// <returns>должен вернуть номер следующего в массиве следующих состояний, НЕ АЙДИ</returns>
        public abstract Task<int> Activate(Entity entity, State previous, IEditableState.Properties properties);
    }

    public interface IOneExitState
    {
    }
}