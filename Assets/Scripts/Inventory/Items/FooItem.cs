using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Foo Item", menuName = "Inventory/Items/Foo", order = 0)]
    public class FooItem : ScriptableObject, IItem
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int MaxStackSize { get; private set; }
        public ScriptableObject SelfRef => this;

        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
    }
}