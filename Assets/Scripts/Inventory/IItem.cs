using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// это предмет для инвентаря
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// ссылка на самого себя
        /// </summary>
        ScriptableObject SelfRef { get; }

        /// <summary>
        /// отображаемое имя предмета
        /// </summary>
        string Name { get; }

        /// <summary>
        /// максимальное колличество в стаке
        /// </summary>
        int MaxStackSize { get; }

        /// <summary>
        /// спрайт для отрисовки
        /// </summary>
        Sprite Sprite { get; }
    }
}