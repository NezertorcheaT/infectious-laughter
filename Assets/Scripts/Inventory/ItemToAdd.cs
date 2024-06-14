using Inventory.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Inventory
{
    [AddComponentMenu("Inventory/Item to add")]
    public class ItemToAdd : MonoBehaviour, IItemAdder
    {
        [SerializeField] private PlayerInventoryInput input;
        [SerializeField] private ScriptableObject item;
        [SerializeField] private LayerMask layer;
        [Inject] private Controls _actions;

        private void OnPickItem(InputAction.CallbackContext ctx)
        {
            if (Vector2.Distance(input.Entity.CachedTransform.position, transform.position) > input.MaxDistance) return;
            var entityPosition = input.Entity.CachedTransform.position;
            var substr = transform.position - entityPosition;
            Debug.DrawRay(entityPosition, substr);

            if (Physics2D.Raycast(entityPosition, substr.normalized, substr.magnitude, layer)) return;
            input.AddItem(item);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            _actions.Gameplay.PickItem.performed -= OnPickItem;
        }

        private void OnEnable()
        {
            _actions.Gameplay.PickItem.performed += OnPickItem;
        }

        IItem IItemAdder.Item
        {
            get => item as IItem;
            set => item = value.SelfRef;
        }

        IInventoryInput IItemAdder.Input
        {
            get => input;
            set => input = value as PlayerInventoryInput;
        }
    }
}