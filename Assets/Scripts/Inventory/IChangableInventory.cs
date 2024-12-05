using System;

namespace Inventory
{
    /// <summary>
    /// инвентарь, поддерживающий добавление в него предметов
    /// </summary>
    public interface IChangableInventory : IInventory
    {
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
    }
}