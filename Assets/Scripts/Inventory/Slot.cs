using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Slot : ISlot
    {
        [field: SerializeField]
        public int Count
        {
            get => Mathf.Clamp(_count, 0, LastItem.MaxStackSize);
            private set => _count = Mathf.Clamp(value, 0, LastItem.MaxStackSize);
        }

        public int _count = 0;

        public bool IsEmpty => LastItem == null || Count == 0;

        public IItem LastItem
        {
            get => _lastItem as IItem;
            private set => _lastItem = value as ScriptableObject;
        }

        [SerializeField] private ScriptableObject _lastItem;

        public void Use(Entity.Entity entity)
        {
            var usableItem = (IUsableItem) LastItem;
            if (usableItem == null) return;

            usableItem.Use(entity);
            Count--;
            if (Count == 0)
                LastItem = null;
        }

        public Slot(IItem item, int count)
        {
            LastItem = item;
            Count = item != null ? count : 0;
        }
    }
}