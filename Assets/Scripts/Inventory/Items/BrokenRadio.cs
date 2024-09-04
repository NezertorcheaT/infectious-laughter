using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Broken Radio", menuName = "Inventory/Items/Broken Radio", order = 0)]
    public class BrokenRadio : ScriptableObject, IItem
    {
        public string Name => "Broken Radio";
        public string Id => "il.broken_radio";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
    }
}