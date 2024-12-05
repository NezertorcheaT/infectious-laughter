namespace Inventory
{
    /// <summary>
    /// инвентарь, поддерживающий использование предметов в нём
    /// </summary>
    public interface IUsableInventory : IInventory
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
}