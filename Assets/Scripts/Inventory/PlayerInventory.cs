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
                    if (_slots[i].LastItem is not IStackableItem stackableItem) continue;
                    if (stackableItem.GetType().Name != item.GetType().Name) continue;
                    if (stackableItem.SelfRef != item.SelfRef) continue;
                    if (stackableItem.Id != item.Id) continue;
                    if (stackableItem is IStackableClampedItem clamped &&
                        _slots[i].Count >= clamped.MaxStackSize) continue;

                    if (!addItem) return true;
                    _slots[i].InitializeInventory(this);
                    _slots[i].Count++;
                    slot = _slots[i];
                    OnChange?.Invoke();
                    return true;
                }
            }

            var cur = _slots.FirstOrDefault(i => i.IsEmpty);
            if (cur is null) return false;

            if (!addItem) return true;
            cur.InitializeInventory(this);
            cur.LastItem = item;
            cur.Count = 1;
            slot = cur;
            OnChange?.Invoke();
            return true;
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
                var slot = _slots[i];
                if (slot.IsEmpty) continue;
                slot.InitializeInventory(this);
                if (slot.LastItem is IStashingItem stashingItem)
                    foreach (var slotable in slot.Where(i => stashingItem.HasStored(i)))
                        stashingItem.OnEnded(Holder, this, slotable);
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
        private class Slot : ISlot
        {
            [SerializeField] private string lastItemId;
            [SerializeField] private int count;

            public bool IsEmpty => LastItem == null || Count == 0;
            public IInventory HolderInventory => Inventory;
            public PlayerInventory Inventory { get; private set; }

            public int Count
            {
                get => LastItem is null
                    ? 0
                    : LastItem is IStackableClampedItem clamped
                        ? Mathf.Clamp(count, 0, clamped.MaxStackSize)
                        : count;
                set
                {
                    var lastItem = LastItem;
                    if (lastItem is null) return;

                    value = lastItem is IStackableClampedItem clamped
                        ? Mathf.Clamp(value, 0, clamped.MaxStackSize)
                        : Mathf.Max(value, 0);

                    if (count == value) return;

                    if (lastItem is not IStackableItem)
                    {
                        value = value == 0 ? 0 : 1;
                        if (count == value) return;

                        if (lastItem is IStartableItem startable && count == 0 && value == 1)
                            startable.OnStart(Inventory.Holder, Inventory, new ItemData(startable, this, 1));
                        if (lastItem is IEndableItem endable && count == 1 && value == 0)
                            endable.OnEnded(Inventory.Holder, Inventory, new ItemData(endable, this, 1));

                        count = value;
                        Inventory.OnChange?.Invoke();
                        return;
                    }

                    if (count < value && lastItem is IStartableItem e)
                        for (var i = 1; i <= value - count; i++)
                            e.OnStart(Inventory.Holder, Inventory, new ItemData(e, this, i + count));
                    if (count > value && lastItem is IEndableItem s)
                        for (var i = 1; i <= count - value; i++)
                            s.OnEnded(Inventory.Holder, Inventory, new ItemData(s, this, i + value));

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
                usableItem.Use(entity, Inventory, new ItemData(usableItem, this, 1));
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

            public static bool operator ==(ISlot left, Slot right) => left != null && left.Equals(right);
            public static bool operator !=(ISlot left, Slot right) => !(left != null && left.Equals(right));
        }
    }
}