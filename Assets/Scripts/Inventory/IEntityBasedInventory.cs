using System;

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
}