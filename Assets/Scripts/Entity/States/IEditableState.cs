using System;
using UnityEngine;

namespace Entity.States
{
    /// <summary>
    /// это интерфейс для состояния<br />
    /// он позволяет использовать разные параметры для одинаковых стейтов<br />
    /// при его реализации обязательно внутри состояния создать класс, наследованый от <c>Properties</c>, который [<c>Serializable</c>], это ОБЯЗАТЕЛЬНО<br />
    /// пример реализации вы найдёте в <c>WaitState</c>
    /// </summary>
    public interface IEditableState
    {
        /// <summary>
        /// этот метод используется деревом для получения типа класса, наследованого от <c>Properties</c>, о котором говорилось ранее
        /// </summary>
        /// <returns>тип класса, наследованого от Properties, о котором говорилось ранее</returns>
        Type GetTypeOfEdit();

        /// <summary>
        /// класс, о котором говорилось ранее, представляет собой класс (ахуеть), наследуемый от этого <c>Properties</c><br />
        /// таким образом стейт будет получать данные из дерева<br />
        /// [<c>Serializable</c>] ОБЯЗАТЕЛЬНО
        /// </summary>
        abstract class Properties : ScriptableObject
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
}