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
        /// /// <param isStackable="bool">переменная для проверки, со стакающимися обектами. Ставим фалс когда не надо првоерят стакающиеся обекты</param>
        /// /// <param addItem="bool">переменная, надо ли добавлять обект, или надо вернуть переменную которая показывает можем добавить или нет</param>
        /// <param name="slot">это слот, в который был помешён предмет</param>
        /// <returns>вошло ли</returns>
        bool TryAddItem(IItem item, out ISlot slot, bool isStackable = true, bool addItem = true);

        /// <summary>
        /// попытаться пропихнуть предмет
        /// </summary>
        /// <param name="item">предмет для пропихона</param>
        /// /// <param isStackable="bool">переменная для проверки, со стакающимися обектами. Ставим фалс когда не надо првоерят стакающиеся обекты</param>
        /// /// <param addItem="bool">переменная, надо ли добавлять обект, или надо вернуть переменную которая показывает можем добавить или нет</param>
        /// <returns>вошло ли</returns>
        bool TryAddItem(IItem item, bool isStackable = true, bool addItem = true);

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