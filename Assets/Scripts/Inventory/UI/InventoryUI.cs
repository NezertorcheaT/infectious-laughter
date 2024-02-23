using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    [AddComponentMenu("Inventory/Inventory UI")]
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private HorizontalLayoutGroup inventoryBase;
        [SerializeField, Min(1)] private float imageSize;
        private IInventory _inventory;
        private bool _hasOnChange;
        private void Start()
        {
            if (inventory is IInventory inv) _inventory = inv;
            if (_inventory is not null && !_hasOnChange) _inventory.OnChange += UpdateGUI;
            _hasOnChange = true;
            UpdateGUI();
        }

        private void OnEnable()
        {
            if (_inventory is not null && !_hasOnChange) _inventory.OnChange += UpdateGUI;
            _hasOnChange = true;
        }

        private void OnDisable()
        {
            if (_inventory is not null && _hasOnChange) _inventory.OnChange -= UpdateGUI;
            _hasOnChange = false;
        }

        private void UpdateGUI()
        {
            for (var i = 0; i < inventoryBase.transform.childCount; i++)
            {
                Destroy(inventoryBase.transform.GetChild(i).gameObject);
            }

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