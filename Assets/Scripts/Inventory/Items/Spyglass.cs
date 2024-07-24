using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "Spyglass Item", menuName = "Inventory/Items/Spyglass", order = 0)]
    public class Spyglass : ScriptableObject, IUsableItem
    {
        public string Name => "Spyglass";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private Sprite sprite;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            entity.FindExactAbilityByType<Entity.Abilities.PlayerCameraFollowPointAbility>()?.ChangeLock();
        }
    }
}