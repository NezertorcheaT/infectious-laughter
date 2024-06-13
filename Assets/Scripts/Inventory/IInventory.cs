using System;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// эээ нууу инвентарь
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// максимальное колличество слотов
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// колличество занятых слотов
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// это вот слоты
        /// </summary>
        List<ISlot> Slots { get; }

        /// <summary>
        /// при изменении инвентаря
        /// </summary>
        event Action OnChange;

        /// <summary>
        /// попытаться пропихнуть предмет
        /// </summary>
        /// <param name="item">предмет для пропихона</param>
        /// <returns>вошло ли</returns>
        bool TryAddItem(IItem item);

        /// <summary>
        /// использовать предмет на слоте
        /// </summary>
        /// <param name="i">номер слота</param>
        /// <param name="entity">сущность куда</param>
        void UseItemOnSlot(int i, Entity.Entity entity);

        /// <summary>
        /// использовать последний предмет
        /// </summary>
        /// <param name="entity">сущность куда</param>
        void UseLast(Entity.Entity entity);

        ///<summary>
        /// ну вот выделить определенную ячейку
        ///</summary>
        void SelectingSlot(int slotForSelectNum);

        ///<summary>
        /// Использовать выделенный предмет
        ///</summary>
        void UseSelectItem(Entity.Entity entity);
    }
}