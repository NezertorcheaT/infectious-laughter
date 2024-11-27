namespace Inventory
{
    /// <summary>
    /// предмет, который срабатывает(использование вызывается извне)
    /// </summary>
    public interface ITriggeredItem : IItem
    {
        /// <summary>
        /// вызвать срабатывание предмета
        /// </summary>
        /// <param name="entity">сущность куда</param>
        /// <param name="inventory">предмет откуда</param>
        /// <param name="slot">предмет где</param>
        void Trigger(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}