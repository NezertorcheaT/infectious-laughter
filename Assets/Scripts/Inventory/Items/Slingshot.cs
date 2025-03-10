using AnimationControllers;
using PropsImpact;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Slingshot", menuName = "Inventory/Items/Slingshot", order = 0)]
    public class Slingshot : ScriptableObject, IUsableItem, ICanSpawn, IShopItem, ISpriteItem, IStackableClampedItem
    {
        public string Name => "Slingshot";
        public string Id => "il.slingshot";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public ItemAdderVerifier Verifier { get; set; }

        [SerializeField] private GameObject rockSpawnerPrefab;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        [SerializeField] private int useCount;
        [SerializeField] private float chargeTime;
        [SerializeField] private float upForce;
        [SerializeField] private float minForce;
        [SerializeField] private float maxForce;
        [SerializeField] private int damage;
        [SerializeField] private int stunTime;

        private int _useCount;

        public void Use(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            _useCount--;

            var spawner = Verifier.Container.InstantiatePrefab(
                rockSpawnerPrefab,
                entity.transform.position,
                Quaternion.identity,
                null
            );
            var slingshotImpact = spawner.GetComponent<SlingshotImpact>();
            slingshotImpact.Initialize(entity, upForce, chargeTime, minForce, maxForce, damage, stunTime);
            spawner.GetComponent<SlingshotAnimationController>().Initialize(
                slingshotImpact,
                entity.transform.GetComponent<Animator>(),
                chargeTime
            );
            slingshotImpact.Impact();

            if (_useCount > 0) return;
            itemData.Slot.Count--;
            _useCount = useCount;
        }
    }
}