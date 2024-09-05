using Inventory.Input;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Broken Radio", menuName = "Inventory/Items/Broken Radio", order = 0)]
    public class BrokenRadio : ScriptableObject, IUsableItem, ICanSpawn
    {
        public string Name => "Broken Radio";
        public string Id => "il.broken_radio";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public ItemAdderVerifier Verifier { get; set; }

        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject radioWorld;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var position = entity.gameObject.transform.position;
            var radio = Verifier.Container.InstantiatePrefab(radioWorld, position, Quaternion.identity, null);
            radio.transform.SetParent(null);
            var adder = radio.GetComponent<IItemAdder>();
            adder.Input ??= entity.FindAvailableAbilityByInterface<IInventoryInput>();
            slot.Count--;
        }
    }
}