using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public interface IStackableClampedItem : IStackableItem
    {
        /// <summary>
        /// максимальное колличество в стаке
        /// </summary>
        int MaxStackSize { get; }
    }

    public interface IStackableItem : IItem
    {
    }

    public interface INameableItem : IItem
    {
        /// <summary>
        /// отображаемое имя предмета
        /// </summary>
        string Name { get; }
    }

    public interface ISpriteItem : IItem
    {
        /// <summary>
        /// спрайт для отрисовки
        /// </summary>
        Sprite Sprite { get; }
    }

    /// <summary>
    /// это предмет для инвентаря
    /// </summary>
    public interface IItem : IEquatable<IItem>, IEqualityComparer<IItem>
    {
        /// <summary>
        /// ссылка на самого себя
        /// </summary>
        ScriptableObject SelfRef { get; }

        /// <summary>
        /// ID предмета для сохранения
        /// </summary>
        string Id { get; }

        bool IEquatable<IItem>.Equals(IItem other) => other != null && Id == other.Id;

        bool IEqualityComparer<IItem>.Equals(IItem x, IItem y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        int IEqualityComparer<IItem>.GetHashCode(IItem obj) => HashCode.Combine(obj.Id);
    }
}