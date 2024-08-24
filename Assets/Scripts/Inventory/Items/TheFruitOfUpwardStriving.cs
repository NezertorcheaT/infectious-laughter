using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New The Fruit Of Upward Striving Item", menuName = "Inventory/Items/The Fruit Of Upward Striving", order = 0)]
    public class TheFruitOfUpwardStriving : ScriptableObject, IItem, IUsableItem
    {
        public string Name => "The Fruit Of Upward Striving";
        public string Id => "il.the_fruit_of_upward_striving";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        private Entity.Abilities.Jump _playerJump;
        private float _chachedHeight;
        private bool _used = false;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _playerJump ??= entity.gameObject.GetComponent<Entity.Abilities.Jump>();
            _chachedHeight = _chachedHeight == 0 ? _playerJump.jumpHeight : _chachedHeight;
            _playerJump.jumpHeight = _used ? _chachedHeight : _chachedHeight * 1.5f;
            _used = !_used;
        }
    }
}