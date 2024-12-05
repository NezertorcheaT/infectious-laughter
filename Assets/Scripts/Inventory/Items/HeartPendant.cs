using System;
using Entity.Abilities;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Heart Pendant", menuName = "Inventory/Items/Heart Pendant", order = 0)]
    public class HeartPendant : StashingItem<HeartPendant.Eventer>, IShopItem
    {
        public override string Name => "Heart Pendant";
        public override string Id => "il.heart_pendant";
        public override ScriptableObject SelfRef => this;
        public override Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 2;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private Sprite sprite;
        [SerializeField, Min(1)] private int healAmount = 1;
        [SerializeField, Min(1)] private int maxStackSize = 1;
        public override int MaxStackSize => maxStackSize;

        private static int _healAmount;

        protected override Eventer Initiate(Entity.Entity entity, IInventory inventory, ISlot slot) =>
            new(entity, slot);

        protected override void Started(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _healAmount = healAmount;
        }

        protected override void End(Entity.Entity entity, IInventory inventory, ISlot slot, Eventer current)
        {
            current.Dispose();
        }

        public class Eventer : IDisposable
        {
            private readonly Hp _hp;
            private readonly ISlot _slot;

            public Eventer(Entity.Entity entity, ISlot slot)
            {
                _hp = entity.FindAbilityByType<Hp>();
                _slot = slot;
                _hp.BeforeDie += CheckForHealth;
            }

            private void CheckForHealth()
            {
                if (_hp.Health > 0) return;
                _hp.Health += _healAmount;
                _slot.Count -= 1;
            }

            public void Dispose() => _hp.BeforeDie -= CheckForHealth;
        }
    }
}