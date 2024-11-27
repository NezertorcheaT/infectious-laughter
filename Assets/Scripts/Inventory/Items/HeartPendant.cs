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

        private void Awake()
        {

        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            _currentHealth.OnDamaged -= CheckForHealth;
        }

        private void CheckForHealth(int health, int addictiveHealth, int maxAddictiveHealth, int maxHealth)
        {
            if (_currentHealth.Health < CriticalHealth)
                Trigger(_playerEntity, _inventory, _slot);
        }

        public void Trigger(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            Debug.Log("Death test");
            _currentHealth.Heal(HealAmount);
            foreach(var _slot in inventory.Slots)
            {
                int i = 0;
                if (_slot == slot) inventory.UseItemOnSlot(i, entity);
                i++;
            }
            slot.Count--;
        }

        public void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _playerEntity = entity;
            _inventory = inventory;
            _slot = slot;
            _currentHealth = entity.GetComponent<Hp>();
            _currentHealth.OnDamaged += CheckForHealth;
        }
    }
}