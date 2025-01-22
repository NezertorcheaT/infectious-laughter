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
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором лежит предмет</param>
        void Use(Entity.Entity entity, IInventory inventory,ItemData itemData);
    }
}