using System;
using System.Threading.Tasks;
using GameFlow;
using Installers;
using Inventory;
using Inventory.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private ShopItemFrame[] itemFrames;

        [SerializeField, Min(0), Tooltip("в секундах")]
        private float frameShowDelay;

        [SerializeField] private Image background;
        [SerializeField] private GameObject backPanel;
        [SerializeField] private PopUp openPanel;
        [Inject] private PlayerInstallation _playerInstallation;
        [Inject] private GarbageManager _garbageManager;
        [Inject] private Controls _controls;
        private PlayerInventoryInput _playerInventoryInput;
        private bool _isShopClosed;

        private void Start()
        {
            _playerInventoryInput = _playerInstallation.Entity.FindAbilityByType<PlayerInventoryInput>();
            backPanel?.SetActive(false);
        }

        private void OnEnable() => _controls.Gameplay.CloseShop.performed += KeyCloseShop;
        private void OnDisable() => _controls.Gameplay.CloseShop.performed -= KeyCloseShop;
        private void KeyCloseShop(InputAction.CallbackContext callbackContext) => CloseShop();

        public async void CloseShop()
        {
            foreach (var frame in itemFrames)
            {
                await Task.Delay(TimeSpan.FromSeconds(frameShowDelay));
                frame.Stop();
                frame.gameObject.SetActive(false);
            }

            await Task.Delay(TimeSpan.FromSeconds(frameShowDelay));
            background.enabled = false;
            backPanel?.SetActive(false);
            openPanel.enabled = true;
            _isShopClosed = true;
        }

        public void OpenShop()
        {
            _isShopClosed = false;
            backPanel.SetActive(true);
            background.enabled = true;

            foreach (var frame in itemFrames)
            {
                frame.gameObject.SetActive(true);
                frame.Animate();
            }
        }

        public void SetShopwindow(IInventory shopInventory)
        {
            var i = 0;
            foreach (var slot in shopInventory.Slots)
            {
                var frame = itemFrames[i];
                i++;
                if (slot.LastItem is not IShopItem shopItem)
                    continue;

                frame.Item.sprite = shopItem.SpriteForShop;
                frame.Text.SetText($"{shopItem.Name}\nЗа {shopItem.ItemCost} мусора");
                frame.Button.onClick.AddListener(() => OnButtonClick(frame, slot));
            }
        }

        private void OnButtonClick(ShopItemFrame frame, ISlot slot)
        {
            //Реализация покупки предмета
            if (slot.LastItem is not IShopItem shopItem) return;
            if (!_garbageManager.IfCanAfford(shopItem.ItemCost) || slot.IsEmpty) return;
            if (!_playerInventoryInput.HasSpace(shopItem)) return;

            _garbageManager.GarbageBalance -= shopItem.ItemCost;
            slot.Count = 0;

            _playerInventoryInput.AddItem(shopItem.SelfRef);

            frame.Text.SetText($"{shopItem.Name}\nПродано");
        }
    }
}