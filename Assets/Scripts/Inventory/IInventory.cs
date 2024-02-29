using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public interface IInventory
    {
        int MaxCapacity { get; }
        int Capacity { get; }
        List<ISlot> Slots { get; }
        event Action OnChange;
        bool TryAddItem(IItem item);
        void UseItemOnSlot(int i, Entity.Entity entity);
        void UseLast(Entity.Entity entity);
    }

    public interface IItem
    {
        ScriptableObject SelfRef{ get; }
        string Name { get; }
        int MaxStackSize { get; }
        Sprite Sprite { get; }
    }

    public interface ISlot
    {
        int Count { get; }
        IItem LastItem { get; }
        bool IsEmpty { get; }
        void Use(Entity.Entity entity);
    }

    public interface IUsableItem : IItem
    {
        void Use(Entity.Entity entity);
    }
}