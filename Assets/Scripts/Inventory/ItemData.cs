using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// единовременное представление образа предмета для слота
    /// </summary>
    public readonly struct ItemData : IEquatable<ItemData>, IEqualityComparer<ItemData>
    {
        public bool Equals(ItemData other) =>
            Item.Id.Equals(other.Item.Id)
            && Slot.Equals(other.Slot)
            && Position == other.Position;

        public bool Equals(ItemData x, ItemData y) => x.Equals(y);
        public override bool Equals(object obj) => obj is ItemData other && Equals(other);

        public int GetHashCode(ItemData obj) => obj.GetHashCode();
        public override int GetHashCode() => HashCode.Combine(Slot, Item.Id, Position);

        public static bool operator ==(ItemData left, ItemData right) => left.Equals(right);
        public static bool operator !=(ItemData left, ItemData right) => !left.Equals(right);

        /// <summary>
        /// это там где лежит предмет
        /// </summary>
        [NotNull]
        public ISlot Slot { get; }

        /// <summary>
        /// тип предмета
        /// </summary>
        [NotNull]
        public IItem Item { get; }

        /// <summary>
        /// позиция предмета в слоте
        /// </summary>
        public int Position { get; }

        public ItemData([NotNull] IItem item, [NotNull] ISlot slot, int position)
        {
            Item = item;
            Slot = slot;
            Position = Mathf.Max(0, position);
        }

        public override string ToString() =>
            $"{nameof(ItemData)} {{ Slot = {Slot}, Item = {Item.Id}, Position = {Position} }}";
    }
}