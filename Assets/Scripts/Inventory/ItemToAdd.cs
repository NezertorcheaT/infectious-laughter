using Inventory.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Inventory
{
    [AddComponentMenu("Inventory/Item to add")]
    public class ItemToAdd : MonoBehaviour
    {
        [SerializeField] private InventoryInput input;
        [SerializeField] private ScriptableObject item;
        [Inject] private Controls _actions;

        private void OnPickItem(InputAction.CallbackContext ctx)
        {
            if (Vector2.Distance(input.Entity.CachedTransform.position, transform.position) > input.MaxDistance) return;
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
    }
}