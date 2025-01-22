using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Fruit Of Aspiration", menuName = "Inventory/Items/Fruit Of Aspiration", order = 0)]
    public class FruitOfAspiration : ScriptableObject, INameableItem, ISpriteItem, IStartableItem, IEndableItem
    {
        public string Name => "Fruit Of Aspiration";
        public string Id => "il.fruit_of_aspiration";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [SerializeField] private float speedMultiplier = 1.5f;

        private Entity.Abilities.HorizontalMovement _playerMovement;

        public void OnStart(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            _playerMovement = entity.GetComponent<Entity.Abilities.HorizontalMovement>();
            _playerMovement.Speed *= speedMultiplier;
        }

        public void OnEnded(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            _playerMovement = entity.GetComponent<Entity.Abilities.HorizontalMovement>();
            _playerMovement.Speed /= speedMultiplier;
        }
    }
}