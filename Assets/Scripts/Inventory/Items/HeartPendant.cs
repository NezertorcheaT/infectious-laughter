using System;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Heart Pendant", menuName = "Inventory/Items/Heart Pendant", order = 0)]
    public class HeartPendant : ScriptableObject, IShopItem, IStartableItem, IEndableItem, ICanSpawn
    {
        public string Name => "Heart Pendant";
        public string Id => "il.heart_pendant";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public ItemAdderVerifier Verifier { get; set; }

        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject transportPrefab;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
        [field: SerializeField, Min(1)] private int healAmount = 1;

        private Dictionary<Entity.Entity, Currency> _currents;
        private static int _healAmount;

        public void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _healAmount = healAmount;
            _currents ??= new Dictionary<Entity.Entity, Currency>();
            _currents.Add(entity, new Currency(entity, slot));
        }

        public void OnEnded(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var position = entity.transform.position;
            var transport = Verifier.Container.InstantiatePrefab(transportPrefab, position, Quaternion.identity, null);
            _currents.Remove(entity, out var c);
            c.Dispose();
        }

        private class Currency : IDisposable
        {
            private readonly Hp _hp;
            private readonly ISlot _slot;

            public Currency(Entity.Entity entity, ISlot slot)
            {
                _hp = entity.GetComponent<Hp>();
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