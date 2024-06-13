using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Trap Item", menuName = "Inventory/Items/Trap", order = 0)]
    public class TrapItem : ScriptableObject, IUsableItem
    {
        public string Name => "Trap";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }
        [field: SerializeField] public GameObject TrapWorld { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var trap = Instantiate(TrapWorld, entity.CachedTransform.position, Quaternion.identity);
            trap.transform.SetParent(null);
            slot.Count--;
        }
    }
}