using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Inventory.UI
{
    [AddComponentMenu("Inventory/Inventory UI")]
    public class InventoryUI : MonoBehaviour
    {
        [Inject] private Controls _actions;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Entity.Entity player;
        [SerializeField] private HorizontalLayoutGroup inventoryBase;
        [SerializeField, Min(1)] private float imageSize;
        private IInventory _inventory => inventory as IInventory;

        private void Start()
        {
            UpdateGUI();
        }

        private void OnEnable()
        {
            if (_inventory is null) return;
            _inventory.OnChange += UpdateGUI;
            _actions.Gameplay.PickGarbage.performed += UseItem;
        }

        private void OnDisable()
        {
            if (_inventory is null) return;
            _inventory.OnChange -= UpdateGUI;
            _actions.Gameplay.PickGarbage.performed -= UseItem;
        }

        private void UseItem(InputAction.CallbackContext ctx) => _inventory.UseLast(player);

        private void UpdateGUI()
        {
            for (var i = 0; i < inventoryBase.transform.childCount; i++)
            {
                Destroy(inventoryBase.transform.GetChild(i).gameObject);
            }

            if (_inventory is null) return;

            foreach (var slot in _inventory.Slots)
            {
                var go = new GameObject();
                go.transform.SetParent(inventoryBase.transform);
                var image = go.AddComponent<Image>();
                var rect = go.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(imageSize, imageSize);

                if (slot.IsEmpty)
                {
                    go.name = "Nothing";
                    continue;
                }

                go.name = $"{slot.LastItem.Name} by {slot.Count}";
                image.sprite = slot.LastItem.Sprite;
            }
        }
    }
}