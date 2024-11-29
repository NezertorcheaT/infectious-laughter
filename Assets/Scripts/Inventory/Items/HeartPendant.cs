using Entity.Abilities;
using Installers;
using UnityEngine;
using Zenject;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Heart Pendant", menuName = "Inventory/Items/Heart Pendant", order = 0)]
    public class HeartPendant : ScriptableObject, ITriggeredItem, IShopItem, IStartableItem
    {
        public string Name => "Heart Pendant";
        public string Id => "il.heart_pendant";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
        [field: SerializeField, Min(1)] private int CriticalHealth = 1;
        [field: SerializeField, Min(1)] private int HealAmount = 1;

        private Hp _currentHealth;

        private Entity.Entity _playerEntity;

        private IInventory _inventory;

        private ISlot _slot;


        public void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _playerEntity = entity;
            _inventory = inventory;
            _slot = slot;
            _currentHealth = entity.GetComponent<Hp>();
            _currentHealth.OnDamaged += CheckForHealth;
        } 
        private void CheckForHealth(int health, int addictiveHealth, int maxAddictiveHealth, int maxHealth)
        {
            if (_currentHealth.Health < CriticalHealth)
            {
                _currentHealth.OnDamaged -= CheckForHealth;
                Trigger(_playerEntity, _inventory, _slot);
            }
        }

        public void Trigger(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
        //_currentHealth.Heal(HealAmount);
        /* проблемна€ строка, оставл€€ еЄ получаем 
        StackOverflowException: The requested operation caused a stack overflow.
        Inventory.ItemsProvider.IdToItem(System.String id)(at Assets / Scripts / Inventory / ItemsProvider.cs:0)
        Inventory.Slot.get_LastItem()(at Assets / Scripts / Inventory / Slot.cs:27)
        Inventory.Slot.Trigger(Entity.Entity entity, Inventory.IInventory inventory)(at Assets / Scripts / Inventory / Slot.cs:41)
        Inventory.Inventory.TriggerItemOnSlot(System.Int32 i, Entity.Entity entity)(at Assets / Scripts / Inventory / Inventory.cs:91)
        */
            var i = inventory.Slots.IndexOf(slot);
            inventory.TriggerItemOnSlot(i, entity);
            slot.Count--;
        }
    }
}