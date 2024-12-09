using System.Collections;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// слот инветаря
    /// </summary>
    public interface ISlot : IEnumerable<Slotable>
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
        void Use(Entity.Entity entity);

        IEnumerator<Slotable> IEnumerable<Slotable>.GetEnumerator()
        {
            if (IsEmpty) yield break;
            var lastItem = LastItem;
            for (var i = 1; i <= Count; i++)
                yield return new Slotable(lastItem, this, i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}