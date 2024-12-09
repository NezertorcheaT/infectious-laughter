using System;
using System.Collections;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// слот инветаря
    /// </summary>
    public interface ISlot : IEnumerable<Slotable>, IEquatable<ISlot>, IEqualityComparer<ISlot>
    {
        /// <summary>
        /// инвентарь слота
        /// </summary>
        IInventory HolderInventory { get; }

        /// <summary>
        /// количество предметов в слоте
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
        void Use(Entity.Entity entity);

        IEnumerator<Slotable> IEnumerable<Slotable>.GetEnumerator()
        {
            if (IsEmpty) yield break;
            var lastItem = LastItem;
            for (var i = 1; i <= Count; i++)
                yield return new Slotable(lastItem, this, i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool IEquatable<ISlot>.Equals(ISlot other)
        {
            if (!ReferenceEquals(this, other)) return false;
            if (GetType() != other.GetType()) return false;
            return Equals(LastItem, other.LastItem) && Count == other.Count && HolderInventory == other.HolderInventory;
        }

        bool IEqualityComparer<ISlot>.Equals(ISlot x, ISlot y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            if (!ReferenceEquals(x, y)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x.LastItem, y.LastItem) && x.Count == y.Count && x.HolderInventory == y.HolderInventory;
        }

        int IEqualityComparer<ISlot>.GetHashCode(ISlot obj) => HashCode.Combine(LastItem.Id, Count, HolderInventory);
    }
}