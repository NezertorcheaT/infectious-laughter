using Entity;
using UnityEngine;

namespace Inventory.Input
{
    public interface IInventoryInput<out T> : IInitializeByEntity where T : IInventory
    {
        T Inventory { get; }
        public void AddItem(ScriptableObject item);
    }
}