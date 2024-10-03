using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Umbrella", menuName = "Inventory/Items/Umbrella", order = 0)]
    public class Umbrella : ScriptableObject, IItem, IUsableItem
    {
        public string Name => "Umbrella";
        public string Id => "il.umbrella";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        private Entity.Abilities.LightResponsive _lightResponse;
        [SerializeField] private int useCount;

        private int _useCount;


        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            if(_lightResponse == null) _useCount = useCount;
            _lightResponse = entity.gameObject.GetComponent<Entity.Abilities.LightResponsive>();
            if(!_lightResponse.Resistance)
            {
                _lightResponse.Resistance = true;
                _useCount--;
                if(_useCount <= 0) slot.Count--;
            }
        }
    }
}