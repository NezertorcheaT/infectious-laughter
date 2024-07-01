using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Slot : ISlot
    {
        public int Count
        {
            get => Mathf.Clamp(_count, 0, LastItem.MaxStackSize);
            set => _count = Mathf.Clamp(value, 0, LastItem.MaxStackSize);
        }

        [SerializeField] private ScriptableObject lastItem;

        public int _count = 0;

        public bool IsEmpty => LastItem == null || Count == 0;

        public IItem LastItem
        {
            get => lastItem as IItem;
            set => lastItem = value?.SelfRef;
        }

        public void Use(Entity.Entity entity, IInventory inventory)
        {
            if (!(LastItem is IUsableItem usableItem)) return;
            usableItem.Use(entity, inventory, this);
            if (Count > 0) return;
            Count = 0;
        }

        public Slot(IItem item, int count)
        {
            LastItem = item;
            Count = item != null ? count : 0;
        }
    }
}