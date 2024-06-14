using Entity;
using UnityEngine;

namespace Inventory.Input
{
    public interface IInventoryInput : IInitializeByEntity
    {
        IInventory Inventory { get; }
        public void AddItem(ScriptableObject item);
    }
}