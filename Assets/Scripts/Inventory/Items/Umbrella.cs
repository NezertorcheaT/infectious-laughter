using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Umbrella", menuName = "Inventory/Items/Umbrella", order = 0)]
    public class Umbrella : ScriptableObject,
        IUsableItem,
        IShopItem,
        ISpriteItem,
        IStackableClampedItem,
        IStashingItem<Umbrella.UmbrellaData>
    {
        public class UmbrellaData
        {
            public UmbrellaData(int useCount)
            {
                UseCount = useCount;
            }

            public int UseCount;
        }

        public string Name => "Umbrella";
        public string Id => "il.umbrella";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private int useCount;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        private Entity.Abilities.LightResponsive _lightResponse;

        public void Use(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            var data = Data[itemData];
            if (!_lightResponse) data.UseCount = useCount;
            _lightResponse = entity.gameObject.GetComponent<Entity.Abilities.LightResponsive>();

            if (_lightResponse.Resistance) return;
            _lightResponse.Resistance = true;
            data.UseCount--;

            if (data.UseCount <= 0)
                itemData.Slot.Count--;
        }

        public void InitializeStash() => Data ??= new IStashingItem<UmbrellaData>.Stash();
        public IStashingItem<UmbrellaData>.Stash Data { get; private set; }

        public UmbrellaData Initiate(
            Entity.Entity entity,
            IInventory inventory,
            ItemData itemData
        ) => new(useCount);

        public void Started(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
        }

        public void End(Entity.Entity entity, IInventory inventory, ItemData itemData, UmbrellaData c)
        {
        }
    }
}