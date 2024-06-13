namespace Inventory
{
    /// <summary>
    /// слот инветаря
    /// </summary>
    public interface ISlot
    {
        /// <summary>
        /// колличество предметов в слоте
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// предмет
        /// </summary>
        IItem LastItem { get; set; }

        /// <summary>
        /// пуст ли слот
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// использовать предмет в слоте
        /// </summary>
        /// <param name="entity">сущность куда</param>
        /// <param name="inventory">слоте, где</param>
        void Use(Entity.Entity entity, IInventory inventory);
    }
}