namespace Inventory
{
    /// <summary>
    /// предмет, который срабатывает(использование вызывается извне)
    /// </summary>
    public interface ITriggeredItem : IItem
    {
        void Trigger();
    }
}