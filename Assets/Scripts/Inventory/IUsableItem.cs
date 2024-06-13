namespace Inventory
{
    /// <summary>
    /// предмет, который можно использовать
    /// </summary>
    public interface IUsableItem : IItem
    {
        /// <summary>
        /// использовать предмет
        /// </summary>
        /// <param name="entity">сущность куда</param>
        /// <param name="inventory">предмет откуда</param>
        /// <param name="slot">предмет где</param>
        void Use(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}