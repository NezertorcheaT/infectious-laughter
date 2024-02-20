using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory", order = 0)]
    public class Inventory : ScriptableObject, IInventory
    {
        [SerializeField] private int maxCapacity;
        public event Action OnChange;

        public int MaxCapacity => maxCapacity;
        public int Capacity => Slots.Count(slot => !slot.IsEmpty);

        public List<ISlot> Slots
        {
            get => _slots.Cast<ISlot>().ToList();
            private set => _slots = value.Cast<Slot>().ToList();
        }

        [field: SerializeField] private List<Slot> _slots { get; set; }

        public bool TryAddItem(IItem item)
        {
            for (var i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].IsEmpty) continue;
                if (Slots[i].LastItem.GetType().Name != item.GetType().Name) continue;
                if (Slots[i].Count >= Slots[i].LastItem.MaxStackSize) continue;

                Slots[i] = new Slot(item, Slots[i].Count + 1);
                OnChange?.Invoke();
                return true;
            }

            for (var i = 0; i < Slots.Count; i++)
            {
                if (!Slots[i].IsEmpty) continue;

                Slots[i] = new Slot(item, 1);
                OnChange?.Invoke();
                return true;
            }

            return false;
        }

        public void UseLast(Entity.Entity entity)
        {
            for (var i = MaxCapacity - 1; i >= 0; i--)
            {
                if (Slots[i].IsEmpty) continue;

                UseItemOnSlot(i, entity);
                return;
            }
        }

        public void UseItemOnSlot(int i, Entity.Entity entity)
        {
            if (i >= MaxCapacity) return;
            if (Slots[i].IsEmpty) return;

            Slots[i].Use(entity);
            OnChange?.Invoke();
        }

        private void Start()
        {
            Slots = new List<ISlot>(MaxCapacity);
            for (var i = 0; i < MaxCapacity; i++)
            {
                Slots.Add(new Slot(null, 0));
            }

            OnChange?.Invoke();
        }
    }
}