using System.Collections;
using UnityEngine;
using Installers;
using Zenject;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "Spyglass Item", menuName = "Inventory/Items/Spyglass", order = 0)]
    public class Spyglass : ScriptableObject, IUsableItem
    {
        public string Name => "Spyglass";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public ItemAdderVerifier Verifier { get; set; }
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        [Inject] private Camera _playerCamera;

        private Entity.Abilities.MovementCameraFollowPointAbility _movementFollowPointAbility = null;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            if(_movementFollowPointAbility == null)
            {
                _movementFollowPointAbility = entity.gameObject.GetComponent<Entity.Abilities.MovementCameraFollowPointAbility>();
            }
            _movementFollowPointAbility.ChangeLock();
        }
    }
}