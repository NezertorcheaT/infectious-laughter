using System;
using Entity.Abilities;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Heart Pendant", menuName = "Inventory/Items/Heart Pendant", order = 0)]
    public class HeartPendant : ScriptableObject, IStashingItem<HeartPendant.Eventer>, IShopItem, ISpriteItem,
        IStackableClampedItem, ICanSpawn
    {
        public string Name => "Heart Pendant";
        public string Id => "il.heart_pendant";
        public ScriptableObject SelfRef => this;
        public IStashingItem<Eventer>.Stash Data { get; private set; }

        [SerializeField] private GameObject transportPrefab;
        [SerializeField, Min(1)] private int healAmount = 1;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 2;
        [field: SerializeField] public Sprite SpriteForShop { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
        public ItemAdderVerifier Verifier { get; set; }

        private static int _healAmount;

        public void InitializeStash() => Data ??= new IStashingItem<Eventer>.Stash();

        public Eventer Initiate(
            Entity.Entity entity,
            IInventory inventory,
            ItemData itemData
        ) => new(entity, itemData);

        public void Started(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            _healAmount = healAmount;
        }

        public void End(
            Entity.Entity entity,
            IInventory inventory,
            ItemData itemData,
            Eventer current
        )
        {
            Verifier.Container.InstantiatePrefab(
                transportPrefab,
                entity.transform.position,
                Quaternion.identity,
                null
            );
            current.Dispose();
        }

        public class Eventer : IDisposable
        {
            private readonly Hp _hp;
            private readonly ItemData _itemData;

            public Eventer(Entity.Entity entity, ItemData itemData)
            {
                _hp = entity.FindAbilityByType<Hp>();
                _itemData = itemData;
                _hp.BeforeDie += CheckForHealth;
            }

            private void CheckForHealth()
            {
                if (_hp.Health > 0) return;
                _hp.Health += _healAmount;
                _itemData.Slot.Count -= 1;
            }

            public void Dispose() => _hp.BeforeDie -= CheckForHealth;
        }
    }
}