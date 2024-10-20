using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Fruit Of Striving Forward", menuName = "Inventory/Items/Fruit Of Striving Forward", order = 0)]
    public class FruitOfStrivingForward : ScriptableObject, IItem, IShopItem
    {
        public string Name => "Fruit Of Striving Forward";
        public string Id => "il.fruit_of_the_tree.striving_forward";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 3;
    }
}