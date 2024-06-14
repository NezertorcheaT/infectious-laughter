using Inventory.Input;

namespace Inventory
{
    public interface IItemAdder
    {
        IItem Item { get; set; }
        IInventoryInput Input { get; set; }
    }
}