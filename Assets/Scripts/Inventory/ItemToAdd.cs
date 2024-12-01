using Installers;
using Inventory.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Inventory
{
    [AddComponentMenu("Inventory/Item to add")]
    public class ItemToAdd : MonoBehaviour, IItemAdder
    {
        [SerializeField] private ScriptableObject item;
        [SerializeField] private LayerMask layer;
        [Inject] private Controls _actions;
        [Inject] private PlayerInstallation _player;
        private PlayerInventoryInput _input;

        private void OnPickItem(InputAction.CallbackContext ctx)
        {
            if (Vector2.Distance(_input.Entity.CachedTransform.position, transform.position) > _input.MaxDistance) return;
            var entityPosition = _input.Entity.CachedTransform.position;
            var substr = transform.position - entityPosition;
            Debug.DrawRay(entityPosition, substr);

            if (Physics2D.Raycast(entityPosition, substr.normalized, substr.magnitude, layer)) return;
            _input.AddItem(item);
            Destroy(gameObject);
        }

        private void Start()
        {
            _input = _player.Entity.FindAbilityByType<PlayerInventoryInput>();
        }

        private void OnDisable()
        {
            _actions.Gameplay.PickItem.performed -= OnPickItem;
        }

        public void OnEnable()
        {
            _actions.Gameplay.PickItem.performed += OnPickItem;
        }

        IItem IItemAdder.Item
        {
            get => item as IItem;
            set => item = value.SelfRef;
        }

        IInventoryInput<PlayerInventory> IItemAdder.Input
        {
            get => _input;
            set => _input = value as PlayerInventoryInput;
        }
    }
}