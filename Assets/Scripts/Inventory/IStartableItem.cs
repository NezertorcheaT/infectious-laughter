namespace Inventory
{
    /// <summary>
    /// предмет, с ивентом при добавлении в инвентарь
    /// </summary>
    public interface IStartableItem : IItem
    {
        /// <summary>
        /// при добавлении в инвентарь
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором лежит предмет</param>
        void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot);
    }

    /// <summary>
    /// предмет, с ивентом при убирании из инвентаря
    /// </summary>
    public interface IEndableItem : IItem
    {
        /// <summary>
        /// при убирании из инвентаря
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором всё ещё лежит предмет</param>
        void OnEnded(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}