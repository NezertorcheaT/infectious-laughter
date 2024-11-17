using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Slot : ISlot
    {
        public int Count
        {
            get => LastItem is null ? 0 : Mathf.Clamp(count, 0, LastItem.MaxStackSize);
            set
            {
                if (LastItem is null)
                    return;
                count = Mathf.Clamp(value, 0, LastItem.MaxStackSize);
            }
        }

        [SerializeField] private string lastItemId;
        [SerializeField] private int count;

        public bool IsEmpty => LastItem == null || Count == 0;

        public IItem LastItem
        {
            get => ItemsProvider.Instance.IdToItem(lastItemId);
            set => lastItemId = value is null ? string.Empty : value.Id;
        }

        public void Use(Entity.Entity entity, IInventory inventory)
        {
            if (LastItem is not IUsableItem usableItem) return;
            usableItem.Use(entity, inventory, this);
            if (Count > 0) return;
            Count = 0;
        }

        public Slot(IItem item, int count)
        {
            lastItemId = item is null ? string.Empty : item.Id;
            Count = item is not null ? count : 0;
        }

        public static Slot Empty => new(null, 0);
    }
}