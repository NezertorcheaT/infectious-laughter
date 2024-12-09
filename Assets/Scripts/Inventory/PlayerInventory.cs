using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Controllers;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory", order = 0)]
    public class PlayerInventory : ScriptableObject, IEntityBasedInventory, IChangableInventory, IUsableInventory
    {
        [SerializeField] private int maxCapacity;

        public event Action OnChange;
        public event Action<Entity.Entity> OnHolderChanged;

        public Entity.Entity Holder { get; private set; }
        public int MaxCapacity => maxCapacity;
        public int Capacity => Slots.Count(slot => !slot.IsEmpty);

        public IList<ISlot> Slots
        {
            get => _slots.Cast<ISlot>().ToList();
            private set => _slots = value.Cast<Slot>().ToList();
        }

        public bool Empty => _slots.All(a => a.IsEmpty);

        [field: SerializeField] private List<Slot> _slots { get; set; }

        public bool TryAddItem(IItem item, bool isStackable = true, bool addItem = true) =>
            TryAddItem(item, out _, isStackable, addItem);

        public bool TryAddItem(IItem item, out ISlot slot, bool isStackable = true, bool addItem = true)
        {
            slot = Slot.Empty(this);
            if (isStackable) // Добавлен способ проверять нужно ли стакать объекты или нет 
            {
                for (var i = 0; i < Slots.Count; i++)
                {
                    if (_slots[i].IsEmpty) continue;
                    if (_slots[i].LastItem.GetType().Name != item.GetType().Name) continue;
                    if (_slots[i].LastItem.SelfRef != item.SelfRef) continue;
                    if (_slots[i].LastItem.Id != item.Id) continue;
                    if (_slots[i].Count >= Slots[i].LastItem.MaxStackSize) continue;

                    if (!addItem) return true;

                    _slots[i].Count++;
                    slot = _slots[i];
                    OnChange?.Invoke();
                    return true;
                }
            }

            for (var i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsEmpty) continue;
                if (!addItem) return true;

                _slots[i].LastItem = item;
                _slots[i].Count = 1;
                slot = _slots[i];
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

        public void ClearInventory()
        {
            for (var i = 0; i < MaxCapacity; i++)
            {
                if (Slots[i].IsEmpty) continue;
                _slots[i].InitializeInventory(this);
                Slots[i].Count = 0;
                Slots[i].LastItem = null;
                _slots[i] = Slot.Empty(this);
            }

            OnChange?.Invoke();
        }

        private void Reset()
        {
            Slots = new List<ISlot>(MaxCapacity);
            for (var i = 0; i < MaxCapacity; i++)
            {
                Slots.Add(new Slot(this, null, 0));
            }

            OnChange?.Invoke();
        }

        public bool Bind(Entity.Entity entity)
        {
            if (entity.Controller is not ControllerInput) return false;
            Holder = entity;
            OnHolderChanged?.Invoke(entity);
            return true;
        }

        public void OnDeserialized()
        {
            Debug.Log("des");
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty) continue;
                slot.InitializeInventory(this);
                if (slot.LastItem is not IStartableItem item) continue;
                foreach (var slotable in slot)
                    item.OnStart(Holder, this, slotable);
            }
        }

        [Serializable]
        private class Slot : ISlot, IEquatable<Slot>
        {
            [SerializeField] private string lastItemId;
            [SerializeField] private int count;

            public bool IsEmpty => LastItem == null || Count == 0;
            public PlayerInventory Inventory { get; private set; }

            public int Count
            {
                get => LastItem is null ? 0 : Mathf.Clamp(count, 0, LastItem.MaxStackSize);
                set
                {
                    if (LastItem is null) return;
                    value = Mathf.Clamp(value, 0, LastItem.MaxStackSize);
                    if (count == value) return;

                    if (count < value && LastItem is IStartableItem e)
                        for (var i = 1; i <= value - count; i++)
                            e.OnStart(Inventory.Holder, Inventory, new Slotable(e, this, i + count));
                    if (count > value && LastItem is IEndableItem s)
                        for (var i = 1; i <= value - count; i++)
                            s.OnEnded(Inventory.Holder, Inventory, new Slotable(s, this, i + value));

                    count = value;
                    Inventory.OnChange?.Invoke();
                }
            }

            public IItem LastItem
            {
                get => ItemsProvider.Instance.IdToItem(lastItemId);
                set
                {
                    if (value is null)
                    {
                        Count = 0;
                        lastItemId = string.Empty;
                        Inventory.OnChange?.Invoke();
                        return;
                    }

                    if (lastItemId == value.Id)
                        return;

                    if (LastItem is IEndableItem e && lastItemId != value.Id)
                    {
                        var c = Count;
                        Count = 0;
                        lastItemId = value.Id;
                        Count = c;
                    }

                    lastItemId = value.Id;
                    Inventory.OnChange?.Invoke();
                }
            }

            public void Use(Entity.Entity entity)
            {
                if (LastItem is not IUsableItem usableItem) return;
                usableItem.Use(entity, Inventory, this);
                if (Count > 0) return;
                Count = 0;
            }

            public Slot([NotNull] IInventory inventory, IItem item, int count)
            {
                Inventory = inventory as PlayerInventory;
                lastItemId = item is null ? string.Empty : item.Id;
                Count = item is not null ? count : 0;
            }

            public void InitializeInventory([NotNull] PlayerInventory inventory) => Inventory ??= inventory;

            public static Slot Empty(IInventory inventory) => new(inventory, null, 0);

            public bool Equals(Slot other)
            {
                if (other is null) return false;
                if (lastItemId == other.lastItemId && count == other.count && Equals(Inventory, other.Inventory))
                    return true;
                return ReferenceEquals(this, other);
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Slot)obj);
            }

            public override int GetHashCode() => HashCode.Combine(lastItemId, count, Inventory);
            public static bool operator ==(Slot left, Slot right) => Equals(left, right);
            public static bool operator !=(Slot left, Slot right) => !Equals(left, right);
        }
    }
}