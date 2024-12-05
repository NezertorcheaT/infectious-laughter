using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// эээ нууу инвентарь
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// максимальное количество слотов
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// количество занятых слотов
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// это вот слоты
        /// </summary>
        IList<ISlot> Slots { get; }

        /// <summary>
        /// пустой ли инвентарь полностью
        /// </summary>
        bool Empty { get; }

        /// <summary>
        /// Очищение инвентаря
        /// </summary>
        void ClearInventory();
    }
}