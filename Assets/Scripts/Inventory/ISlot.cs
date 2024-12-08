using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// слот инветаря
    /// </summary>
    public interface ISlot : IEnumerable<ISlot.Slotable>
    {
        /// <summary>
        /// колличество предметов в слоте
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// предмет
        /// </summary>
        IItem LastItem { get; set; }

        /// <summary>
        /// пуст ли слот
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// использовать предмет в слоте
        /// </summary>
        /// <param name="entity">сущность куда</param>
        /// <param name="inventory">слоте, где</param>
        void Use(Entity.Entity entity);

        /// <summary>
        /// единовременное представление образа предмета, для этого слота
        /// </summary>
        struct Slotable : IEquatable<Slotable>
        {
            public bool Equals(Slotable other) =>
                Item.Id.Equals(other.Item.Id)
                && Slot.Equals(other.Slot)
                && Position == other.Position;

            public override bool Equals(object obj) => obj is Slotable other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(Slot, Item.Id, Position);
            public static bool operator ==(Slotable left, Slotable right) => left.Equals(right);
            public static bool operator !=(Slotable left, Slotable right) => !left.Equals(right);

            [NotNull] public ISlot Slot { get; }
            [NotNull] public IItem Item { get; }
            public int Position { get; }

            public Slotable([NotNull] IItem item, [NotNull] ISlot slot, int position)
            {
                Item = item;
                Slot = slot;
                Position = Mathf.Max(0, position);
            }

            public override string ToString() =>
                $"Slotable {{ Slot = {Slot}, Item = {Item.Id}, Position = {Position} }}";
        }
    }
}