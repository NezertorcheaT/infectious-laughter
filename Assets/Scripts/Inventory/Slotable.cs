using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// единовременное представление образа предмета для слота
    /// </summary>
    public readonly struct Slotable : IEquatable<Slotable>
    {
        public bool Equals(Slotable other) =>
            Item.Id.Equals(other.Item.Id)
            && Slot.Equals(other.Slot)
            && Position == other.Position;

        public override bool Equals(object obj) => obj is Slotable other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Slot, Item.Id, Position);
        public static bool operator ==(Slotable left, Slotable right) => left.Equals(right);
        public static bool operator !=(Slotable left, Slotable right) => !left.Equals(right);

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

        public Slotable([NotNull] IItem item, [NotNull] ISlot slot, int position)
        {
            Item = item;
            Slot = slot;
            Position = Mathf.Max(0, position);
        }

        public override string ToString() =>
            $"{nameof(Slotable)} {{ Slot = {Slot}, Item = {Item.Id}, Position = {Position} }}";
    }
}