using Entity.Abilities;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Trap Item", menuName = "Inventory/Items/Trap", order = 0)]
    public class TrapItem : ScriptableObject, IUsableItem
    {
        public string Name => "Trap";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private float spawnRange = 1.2f;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] private GameObject TrapWorld { get; set; }
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var trap = Instantiate(TrapWorld,
                new Vector2(
                    entity.gameObject.transform.position.x + spawnRange *
                    (entity.FindAbilityByType<EntityMovementHorizontalMove>().RightTurn ? 1 : -1),
                    entity.gameObject.transform.position.y
                ),
                Quaternion.identity
            );
            trap.transform.SetParent(null);
            slot.Count--;
        }
    }
}