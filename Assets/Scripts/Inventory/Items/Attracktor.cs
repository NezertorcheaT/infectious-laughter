using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Attracktor", menuName = "Inventory/Items/Attracktor", order = 0)]
    public class Attracktor : ScriptableObject, IUsableItem, IShopItem, ISpriteItem, IStackableClampedItem
    {
        public string Name => "Attracktor";
        public string Id => "il.attracktor";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public int ItemCost => itemCost;

        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionForce;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField, Min(1)] private int itemCost;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            foreach (var hit in Physics2D.OverlapCircleAll(entity.transform.position, explosionRadius))
            {
                if (hit.attachedRigidbody == null) continue;

                var direction = hit.transform.position - entity.transform.position;
                direction.z = 0;
                hit.attachedRigidbody.AddForce(direction.normalized * explosionForce);
            }

            itemData.Slot.Count--;
        }
    }
}