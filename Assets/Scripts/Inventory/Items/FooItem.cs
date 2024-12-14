#nullable enable
using System;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Foo", menuName = "Inventory/Items/Foo", order = 0)]
    public class FooItem : ScriptableObject, IShopItem, INameableItem, IStackableClampedItem, ISpriteItem
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
    }
}