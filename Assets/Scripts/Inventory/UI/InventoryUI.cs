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
        private int _selection;
        private IInventory Inventory => inventory;

        private void Start()
        {
            UpdateGUI();
        }

        private void OnEnable()
        {
            if (Inventory is null) return;
            Inventory.OnChange += UpdateGUI;
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
            if (Inventory is null) return;
            Inventory.OnChange -= UpdateGUI;
            _actions.Gameplay.PickGarbage.performed -= UseItem;
            _actions.Gameplay.MouseWheel.performed -= CheckWheelSelect;
            _actions.Gameplay.Inv_selectSlot1.performed -= SelectSlotNum1;
            _actions.Gameplay.Inv_selectSlot2.performed -= SelectSlotNum2;
            _actions.Gameplay.Inv_selectSlot3.performed -= SelectSlotNum3;
            _actions.Gameplay.Inv_selectSlot4.performed -= SelectSlotNum4;
            _actions.Gameplay.Inv_selectSlot5.performed -= SelectSlotNum5;
            _actions.Gameplay.Inv_selectSlot6.performed -= SelectSlotNum6;
        }

        private void UseItem(InputAction.CallbackContext ctx) => Inventory.UseItemOnSlot(_selection, player);
        private void SelectSlotNum1(InputAction.CallbackContext ctx) => SelectSlot(0);
        private void SelectSlotNum2(InputAction.CallbackContext ctx) => SelectSlot(1);
        private void SelectSlotNum3(InputAction.CallbackContext ctx) => SelectSlot(2);
        private void SelectSlotNum4(InputAction.CallbackContext ctx) => SelectSlot(3);
        private void SelectSlotNum5(InputAction.CallbackContext ctx) => SelectSlot(4);
        private void SelectSlotNum6(InputAction.CallbackContext ctx) => SelectSlot(5);

        private void CheckWheelSelect(InputAction.CallbackContext ctx)
        {
            var input = _actions.Gameplay.MouseWheel.ReadValue<float>();
            if (input == 0) return;
            SelectSlot(_selection + (input > 0 ? 1 : -1));
        }

        private void SelectSlot(int slot)
        {
            _selection = (int) Mathf.Repeat(slot, Inventory.MaxCapacity);
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            for (var i = 0; i < inventoryBase.transform.childCount; i++)
            {
                Destroy(inventoryBase.transform.GetChild(i).gameObject);
            }

            if (Inventory is null) return;

            foreach (var slot in Inventory.Slots)
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