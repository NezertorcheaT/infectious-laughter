using System;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// инвентарь, строго привязанный к сущности
    /// </summary>
    public interface IEntityBasedInventory : IInventory
    {
        /// <summary>
        /// сущность, к которой привязан инвентарь
        /// </summary>
        Entity.Entity Holder { get; }

        /// <summary>
        /// при изменении сущности привязки
        /// </summary>
        event Action<Entity.Entity> OnHolderChanged;

        /// <summary>
        /// привязать инвентарь к сущности
        /// </summary>
        /// <param name="entity">сущность для привязки</param>
        /// <returns>успешность привязки</returns>
        bool Bind(Entity.Entity entity);
    }

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

    /// <summary>
    /// инвентарь, поддерживающий использование предметов в нём
    /// </summary>
    public interface IUsableInventory
    {
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
    }

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