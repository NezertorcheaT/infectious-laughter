using PropsImpact;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Holy Guillotine", menuName = "Inventory/Items/Holy Guillotine", order = 0)]
    public class HolyGuillotine : ScriptableObject, IUsableItem, ICanSpawn
    {
        public string Name => "Holy Guillotine";
        public string Id => "il.holy_guillotine";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public ItemAdderVerifier Verifier { get; set; }
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private float spawnHeight;
        [SerializeField] private float timeOfAction;
        [SerializeField] private float radius;
        [SerializeField] private GameObject worldGuillotinePrefab;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var position = entity.transform.position;
            var spawnPosition = new Vector2(position.x, position.y + spawnHeight);
            var guillotine =
                Verifier.Container.InstantiatePrefab(worldGuillotinePrefab, spawnPosition, Quaternion.identity, null);

            guillotine.transform.SetParent(null);

            var guillotineComponent = guillotine.GetComponent<GuillotineImpact>();

            guillotineComponent.Impact(entity.transform.position.y - 1.5f, radius, timeOfAction);
            slot.Count--;
        }
    }
}