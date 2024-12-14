using System;
using System.Linq;
using Installers;
using Saving;
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
        [Inject] private PlayerInstallation _player;
        [Inject] private SessionFactory _sessionFactory;
        [SerializeField] private HorizontalLayoutGroup inventoryBase;
        [SerializeField] private ItemFrame frameFirst;
        [SerializeField] private ItemFrame frameMiddle;
        [SerializeField] private ItemFrame frameSelect;
        [SerializeField] private ItemFrame frameLast;
        [SerializeField] private RectTransform foregroundSelector;
        private int _selection;

        public event Action<int> OnSelect;

        private void Start()
        {
            _selection = (int)_sessionFactory.Current[SavedKeys.PlayerInventorySelection].Value;
            OnSelect?.Invoke(_selection);
            UpdateGUI();
        }

        private void OnEnable()
        {
            if (_player.Inventory is null) return;
            _player.Inventory.OnChange += UpdateGUI;
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
            if (_player.Inventory is null) return;
            _player.Inventory.OnChange -= UpdateGUI;
            _actions.Gameplay.PickGarbage.performed -= UseItem;
            _actions.Gameplay.MouseWheel.performed -= CheckWheelSelect;
            _actions.Gameplay.Inv_selectSlot1.performed -= SelectSlotNum1;
            _actions.Gameplay.Inv_selectSlot2.performed -= SelectSlotNum2;
            _actions.Gameplay.Inv_selectSlot3.performed -= SelectSlotNum3;
            _actions.Gameplay.Inv_selectSlot4.performed -= SelectSlotNum4;
            _actions.Gameplay.Inv_selectSlot5.performed -= SelectSlotNum5;
            _actions.Gameplay.Inv_selectSlot6.performed -= SelectSlotNum6;
        }

        private void UseItem(InputAction.CallbackContext ctx) =>
            _player.Inventory.UseItemOnSlot(_selection, _player.Entity);

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
            _selection = (int)Mathf.Repeat(slot, _player.Inventory.MaxCapacity);
            _sessionFactory.Current[SavedKeys.PlayerInventorySelection].Value = _selection;
            OnSelect?.Invoke(_selection);
            UpdateGUI();
        }

        private ItemFrame FrameFromPosition(int i)
        {
            if (_selection == i)
                return frameSelect;
            if (i == 0)
                return frameFirst;
            if (i == _player.Inventory.MaxCapacity - 1)
                return frameLast;

            return frameMiddle;
        }

        private void UpdateGUI()
        {
            for (var i = 0; i < inventoryBase.transform.childCount; i++)
            {
                Destroy(inventoryBase.transform.GetChild(i).gameObject);
            }

            if (_player.Inventory is null) return;

            var j = 0;
            foreach (var slot in _player.Inventory.Slots.Where(i =>
                         i.IsEmpty || i.LastItem is INameableItem and ISpriteItem))
            {
                var frame = Instantiate(FrameFromPosition(j), inventoryBase.transform);
                inventoryBase.CalculateLayoutInputVertical();
                inventoryBase.CalculateLayoutInputHorizontal();
                if (frame is null) continue;

                if (slot.IsEmpty)
                {
                    frame.Item.sprite = null;
                    frame.Item.color = new Color(0, 0, 0, 0);
                    frame.name = "Nothing";
                }
                else
                {
                    frame.name = $"{(slot.LastItem as INameableItem)!.Name} by {slot.Count}";
                    frame.Item.sprite = (slot.LastItem as ISpriteItem)!.Sprite;
                }

                j++;
            }

            foregroundSelector.anchoredPosition = new Vector2(_selection * inventoryBase.spacing,
                foregroundSelector.anchoredPosition.y);
        }
    }
}