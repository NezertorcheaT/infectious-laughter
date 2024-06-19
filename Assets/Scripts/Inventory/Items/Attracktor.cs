using Entity.Abilities;
using Inventory.Input;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "Attracktor", menuName = "Inventory/Items/Attracktor", order = 0)]
    public class Attracktor : ScriptableObject, IUsableItem, ICanSpawn
    {
        public string Name => "Attracktor";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public ItemAdderVerifier Verifier { get; set; }

        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionForce;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            Explosion2D(entity.transform.position, entity);
            slot.Count--;
        }



        void Explosion2D(Vector3 position, Entity.Entity entity)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, explosionRadius);

            foreach(Collider2D hit in colliders)
            {
                if(hit.attachedRigidbody != null)
                {
                    Vector3 direction = hit.transform.position - position;
                    direction.z = 0;

                    
                    hit.attachedRigidbody.AddForce(direction.normalized * explosionForce);	
                }
            }
            //entity.GetComponent<Entity.Abilities.PlayerJumpAbility>().
        }
    }
}