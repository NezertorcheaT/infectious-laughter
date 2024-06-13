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
                if (_slots[i].IsEmpty) continue;
                if (_slots[i].LastItem.GetType().Name != item.GetType().Name) continue;
                if (_slots[i].LastItem.SelfRef != item.SelfRef) continue;
                if (_slots[i].LastItem != item) continue;
                if (_slots[i].Count >= Slots[i].LastItem.MaxStackSize) continue;

                _slots[i] = new Slot(item, _slots[i].Count + 1);
                OnChange?.Invoke();
                return true;
            }

            for (var i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsEmpty) continue;

                _slots[i] = new Slot(item, 1);
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

            Slots[i].Use(entity, this);
            OnChange?.Invoke();
        }

        private void Reset()
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