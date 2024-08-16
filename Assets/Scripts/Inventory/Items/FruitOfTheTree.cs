using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Fruit Of The Tree Item", menuName = "Inventory/Items/Fruit Of The Tree", order = 0)]
    public class FruitOfTheTree : ScriptableObject, IItem
    {
        public string Name => "Fruit Of striving forward";
        public string Id => "il.fruit_of_the_tree.striving_forward";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 3;
    }
}