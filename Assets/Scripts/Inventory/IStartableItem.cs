namespace Inventory
{
    /// <summary>
    /// предмет, с ивентом при добавлении в инветарь
    /// </summary>
    public interface IStartableItem : IItem
    {
        /// <summary>
        /// при добавлении в инвентарь
        /// </summary>
        /// <param name="entity">сущность куда</param>
        /// <param name="inventory">предмет откуда</param>
        /// <param name="slot">предмет где</param>
        void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}