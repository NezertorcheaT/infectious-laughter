using System;
using Inventory.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Entity.Entity entity;
        [SerializeField] private HorizontalLayoutGroup inventoryBase;
        [SerializeField, Min(1)] private float imageSize;
        private InventoryInput _ability;
        private IInventory _inventory;

        private void Start()
        {
            _ability = entity.FindAbilityByType<InventoryInput>();
            if (inventory is IInventory inv) _inventory = inv;
            //if (_inventory is not null) _inventory.OnChange += UpdateGUI;
        }

        private void Update()
        {
            if (!_ability) return;

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