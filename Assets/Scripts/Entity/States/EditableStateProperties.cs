using System;
using UnityEngine;

namespace Entity.States
{
    /// <summary>
    /// класс, о котором говорилось ранее в <c>IEditableState</c>, представляет собой класс (ахуеть), наследуемый от этого <c>EditableStateProperties</c><br />
    /// таким образом стейт будет получать данные из дерева<br />
    /// [<c>Serializable</c>] ОБЯЗАТЕЛЬНО
    /// </summary>
    [Serializable]
    public abstract class EditableStateProperties : ScriptableObject
    {
        /// <summary>
        /// если вдруг не хотите апкастить, используйте этот метод
        /// </summary>
        /// <param name="name">имя поля</param>
        /// <typeparam name="T">тип поля</typeparam>
        /// <returns>значение поля</returns>
        public abstract T Get<T>(string name);

        /// <summary>
        /// если вдруг не хотите апкастить, используйте этот метод<br />
        /// в теории записывает значение в дерево, но я не тестил, так как эти два метода бесполезны
        /// </summary>
        /// <param name="name">имя поля</param>
        /// <param name="value">новое значение поля</param>
        /// <typeparam name="T">тип поля</typeparam>
        public abstract void Set<T>(string name, T value);
    }
}