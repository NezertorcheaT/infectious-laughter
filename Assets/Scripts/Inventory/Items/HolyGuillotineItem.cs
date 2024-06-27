using Entity.Abilities;
using Inventory.Input;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "HolyGuillotineItem", menuName = "Inventory/Items/Holy Guillotine Item", order = 0)]
    public class HolyGuillotineItem : ScriptableObject, IUsableItem, ICanSpawn
    {
        public string Name => "Holy Guillotine Item";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public ItemAdderVerifier Verifier { get; set; }
        
        [SerializeField] private float _spawnHeight;
        [SerializeField] private float _timeOfAction;
        [SerializeField] private float _radius;
        [SerializeField] private GameObject _worldGuillotinePrefab;
        [SerializeField] private Sprite sprite;
        private float _savedPlayerPositionY;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            _savedPlayerPositionY = entity.transform.position.y;
            Vector2 SpawnPosition = new Vector2(entity.transform.position.x, entity.transform.position.y + _spawnHeight);
            var guillotine = Verifier.Container.InstantiatePrefab(_worldGuillotinePrefab, SpawnPosition, Quaternion.identity, null);
            
            guillotine.transform.SetParent(null);
            
            guillotine.GetComponent<GuillotineImpact>()._maxYPosition = entity.transform.position.y - 1.5f;
            guillotine.GetComponent<GuillotineImpact>().entity = entity;
            guillotine.GetComponent<GuillotineImpact>()._levitationTime = _timeOfAction;
            guillotine.GetComponent<GuillotineImpact>()._radius = _radius;
            slot.Count--;
        }
    }
}