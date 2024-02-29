using Entity;
using UnityEngine;

namespace Inventory.Input
{
    [AddComponentMenu("Entity/Abilities/Inventory Picking Ability")]
    public class InventoryInput : Ability
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private float maxDistance = 5f;
        private IInventory _inventory;
        public float MaxDistance => maxDistance;

        protected override void Start()
        {
            if (inventory is IInventory inv) _inventory = inv;
        }

        public void AddItem(ScriptableObject item)
        {
            _inventory?.TryAddItem(item as IItem);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}