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
        int Count { get; }

        /// <summary>
        /// предмет
        /// </summary>
        IItem LastItem { get; }

        /// <summary>
        /// пуст ли слот
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// использовать предмет в слоте
        /// </summary>
        /// <param name="entity">сущность куда</param>
        void Use(Entity.Entity entity);
    }
}