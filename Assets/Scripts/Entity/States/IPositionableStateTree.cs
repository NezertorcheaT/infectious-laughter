using UnityEngine;

namespace Entity.States
{
    /// <summary>
    /// это дерево, стейты которого имеют позицию<br />
    /// нужно только для редактора
    /// </summary>
    public interface IPositionableStateTree : IStateTree
    {
        /// <summary>
        /// попытаться задать позицию стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <param name="position">новая позиция</param>
        /// <returns>получилось записать или нет</returns>
        bool TrySetPosition(int id, Vector2 position);

        /// <summary>
        /// гарантировано получить позицию стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <returns>позиция</returns>
        Vector2 GetPosition(int id);

        /// <summary>
        /// попытаться получить позицию стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <param name="position">ссылка на позицию</param>
        /// <returns>получилось получить или нет</returns>
        bool TryGetPosition(int id, ref Vector2 position);
    }
}