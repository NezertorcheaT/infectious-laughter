using Entity.Abilities;
using Inventory.Input;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Trap", menuName = "Inventory/Items/Trap", order = 0)]
    public class TrapItem : ScriptableObject, IUsableItem, ICanSpawn, IShopItem, ISpriteItem, IStackableClampedItem
    {
        public string Name => "Trap";
        public string Id => "il.trap";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public ItemAdderVerifier Verifier { get; set; }
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private float spawnRange = 1.2f;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private GameObject trapWorld;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            var position = entity.gameObject.transform.position;
            var trap = Verifier.Container.InstantiatePrefab(trapWorld,
                new Vector3(
                    position.x + spawnRange *
                    (entity.FindAbilityByType<HorizontalMovement>().Turn ? 1 : -1),
                    position.y
                ),
                Quaternion.identity,
                null
            );
            trap.transform.SetParent(null);
            var adder = trap.GetComponent<IItemAdder>();
            if (adder is not null)
                adder.Input ??= entity.FindAvailableAbilityByInterface<IInventoryInput<PlayerInventory>>();
            itemData.Slot.Count--;
        }
    }
}