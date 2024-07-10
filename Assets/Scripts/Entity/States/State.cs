using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    /// <summary>
    /// это значт состояние<br />
    /// используется в деревьях состояний для ии в <c>ControllerAI</c><br />
    /// если че добавляйте поля, только если они константы, это связано с их работой<br />
    /// для изменяемых полей используйте <c>IEditableState</c>
    /// </summary>
    [Serializable]
    public abstract class State : ScriptableObject
    {
        /// <summary>
        /// имя для редактора
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// асинхронный метод действия
        /// </summary>
        /// <param name="entity">над кем будет действие произведено</param>
        /// <param name="previous">предыдущее состояние<br />хз может нужно кому</param>
        /// <returns>должен вернуть номер следующего в массиве следующих состояний, НЕ АЙДИ</returns>
        public abstract Task<int> Activate(Entity entity, State previous);
    }

    /// <summary>
    /// состояние с одним выходом, нужно только для редактора<br />
    /// по факту выходов может быть сколько хотите, если будете редачить дерево не через редактор
    /// </summary>
    public interface IOneExitState
    {
    }
}