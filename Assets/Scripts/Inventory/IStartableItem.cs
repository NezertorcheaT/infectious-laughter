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
        void OnEnded(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}