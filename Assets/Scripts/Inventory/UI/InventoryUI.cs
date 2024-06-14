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
            _actions.Gameplay.MouseWheel.performed += CheckWheelSelect;
            _actions.Gameplay.Inv_selectSlot1.performed += SelectSlotNum1;
            _actions.Gameplay.Inv_selectSlot2.performed += SelectSlotNum2;
            _actions.Gameplay.Inv_selectSlot3.performed += SelectSlotNum3;
            _actions.Gameplay.Inv_selectSlot4.performed += SelectSlotNum4;
            _actions.Gameplay.Inv_selectSlot5.performed += SelectSlotNum5;
            _actions.Gameplay.Inv_selectSlot6.performed += SelectSlotNum6;
        }

        private void OnDisable()
        {
            if (_inventory is null) return;
            _inventory.OnChange -= UpdateGUI;
            _actions.Gameplay.PickGarbage.performed -= UseItem;
            _actions.Gameplay.MouseWheel.performed -= CheckWheelSelect;
            _actions.Gameplay.Inv_selectSlot1.performed -= SelectSlotNum1;
            _actions.Gameplay.Inv_selectSlot2.performed -= SelectSlotNum2;
            _actions.Gameplay.Inv_selectSlot3.performed -= SelectSlotNum3;
            _actions.Gameplay.Inv_selectSlot4.performed -= SelectSlotNum4;
            _actions.Gameplay.Inv_selectSlot5.performed -= SelectSlotNum5;
            _actions.Gameplay.Inv_selectSlot6.performed -= SelectSlotNum6;
        }

        private void UseItem(InputAction.CallbackContext ctx) => _inventory.UseSelectItem(player);
        private void SelectSlotNum1(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(2);
        private void SelectSlotNum2(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(3);
        private void SelectSlotNum3(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(4);
        private void SelectSlotNum4(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(5);
        private void SelectSlotNum5(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(6);
        private void SelectSlotNum6(InputAction.CallbackContext ctx) => _inventory.SelectingSlot(7);

        private void CheckWheelSelect(InputAction.CallbackContext ctx)
        {
            if(_actions.Gameplay.MouseWheel.ReadValue<float>() > 0)
            {
                _inventory.SelectingSlot(_inventory.getSelectSlot() + 2);
            }else if(_actions.Gameplay.MouseWheel.ReadValue<float>() < 0)
            {
                _inventory.SelectingSlot(_inventory.getSelectSlot());
            }
        }


    
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