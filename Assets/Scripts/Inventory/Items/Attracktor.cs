using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Attracktor Item", menuName = "Inventory/Items/Attracktor", order = 0)]
    public class Attracktor : ScriptableObject, IUsableItem
    {
        public string Name => "Attracktor";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionForce;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            foreach (var hit in Physics2D.OverlapCircleAll(entity.transform.position, explosionRadius))
            {
                if (hit.attachedRigidbody == null) continue;

                var direction = hit.transform.position - entity.transform.position;
                direction.z = 0;
                hit.attachedRigidbody.AddForce(direction.normalized * explosionForce);
            }

            slot.Count--;
        }
    }
}