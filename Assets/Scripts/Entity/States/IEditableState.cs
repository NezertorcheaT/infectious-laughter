using System;

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
    }
}