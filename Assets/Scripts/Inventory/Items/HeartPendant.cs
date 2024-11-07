using Entity.Abilities;
using Installers;
using UnityEngine;
using Zenject;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Heart Pendant", menuName = "Inventory/Items/Heart Pendant", order = 0)]
    public class HeartPendant : ScriptableObject, IUsableItem
    {
        public string Name => "Heart Pendant";
        public string Id => "il.heart_pendant";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 160;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
        [Inject] private PlayerInstallation _player;
        private Hp _currentHealth;

        private void OnEnable()
        {
            _currentHealth ??= _player.Entity.GetComponent<Hp>();
            //_currentHealth.OnDamaged +=  CheckForHealth;
            _currentHealth.OnDamaged += TestCheckForHealth;
        }

        private void OnDisable()
        {
            _currentHealth ??= _player.Entity.GetComponent<Hp>();
            //_currentHealth.OnDamaged -=  CheckForHealth;
            _currentHealth.OnDamaged -= TestCheckForHealth;
        }

        private void CheckForHealth()
        {

        }

        private void TestCheckForHealth(int health, int addictiveHealth, int maxAddictiveHealth, int maxHealth)
        { 

        }

        private void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {

        }
    }
}