using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Fruit Of Upward Striving", menuName = "Inventory/Items/Fruit Of Upward Striving",
        order = 0)]
    public class FruitOfUpwardStriving : ScriptableObject, IUsableItem, INameableItem, ISpriteItem
    {
        public string Name => "Fruit Of Upward Striving";
        public string Id => "il.fruit_of_upward_striving";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;

        private Entity.Abilities.Jump _playerJump;
        private float _chachedHeight;
        private bool _used = false;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _playerJump ??= entity.gameObject.GetComponent<Entity.Abilities.Jump>();
            _chachedHeight = _chachedHeight == 0 ? _playerJump.jumpForce : _chachedHeight;
            _playerJump.jumpForce = _used ? _chachedHeight : _chachedHeight * 1.5f;
            _used = !_used;
        }
    }
}