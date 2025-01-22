using PropsImpact;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Dart", menuName = "Inventory/Items/Dart", order = 0)]
    public class Dart : ScriptableObject, IUsableItem, IShopItem, ISpriteItem, IStackableClampedItem
    {
        public string Name => "Dart";
        public string Id => "il.dart";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private HomingDartPrefab dartPrefab;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float speed = 1;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        public void Use(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            Instantiate(dartPrefab, entity.gameObject.transform.position + offset, Quaternion.identity)
                .Initialize(speed);
        }
    }
}