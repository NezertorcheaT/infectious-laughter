using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Fruit Of Aspiration", menuName = "Inventory/Items/Fruit Of Aspiration", order = 0)]
    public class FruitOfAspiration : ScriptableObject, IUsableItem
    {
        public string Name => "Fruit Of Aspiration";
        public string Id => "il.fruit_of_aspiration";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;

        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        private Entity.Abilities.HorizontalMovement _playerMovement;
        private float _chachedSpeed;
        private bool _used = false;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _playerMovement ??= entity.gameObject.GetComponent<Entity.Abilities.HorizontalMovement>();
            _chachedSpeed = _chachedSpeed == 0 ? _playerMovement.Speed : _chachedSpeed;
            _playerMovement.Speed = _used ? _chachedSpeed : _chachedSpeed * 1.5f;
            _used = !_used;
        }
    }
}