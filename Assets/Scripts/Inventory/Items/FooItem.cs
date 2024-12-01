#nullable enable
using System;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Foo", menuName = "Inventory/Items/Foo", order = 0)]
    public class FooItem : ScriptableObject, IShopItem
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public string Id => "il.foo";
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;


        public static bool operator ==(FooItem a, FooItem b) => a.Equals(b);
        public static bool operator !=(FooItem a, FooItem b) => !a.Equals(b);

        protected bool Equals(IItem? other)
        {
            if (other is null) return false;
            return Name.Equals(other.Name) && MaxStackSize.Equals(other.MaxStackSize);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IItem)obj);
        }

        public override int GetHashCode() => HashCode.Combine(Name, MaxStackSize);
    }
}