using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Dart", menuName = "Inventory/Items/Dart", order = 0)]
    public class Dart : ScriptableObject, IItem, IUsableItem
    {
        public string Name => "Dart";
        public string Id => "il.dart";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject dartPrefab;
        [SerializeField] private Vector3 offset;
        [SerializeField] private int speed;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;

        private GameObject _spawnedDart;

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _spawnedDart = Instantiate(dartPrefab, entity.gameObject.transform.position + offset, Quaternion.identity);
            _spawnedDart.GetComponent<HomingDartPrefab>().speed = speed;
        }
    }
}