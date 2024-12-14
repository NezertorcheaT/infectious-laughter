using System;
using System.Collections.Generic;
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
        private bool _isShopClosed = true;

        private void Start()
        {
            _playerInventoryInput = _playerInstallation.Entity.FindAbilityByType<PlayerInventoryInput>();
            backPanel?.SetActive(false);
        }

        private void OnEnable() => _controls.Gameplay.CloseShop.performed += KeyCloseShop;
        private void OnDisable() => _controls.Gameplay.CloseShop.performed -= KeyCloseShop;
        private void KeyCloseShop(InputAction.CallbackContext callbackContext) => CloseShop();

        private async void CloseShop()
        {
            if (_isShopClosed) return;
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
            if (!_isShopClosed) return;
            _isShopClosed = false;
            backPanel.SetActive(true);
            background.enabled = true;

            foreach (var frame in itemFrames)
            {
                frame.gameObject.SetActive(true);
                frame.Animate();
            }
        }

        public void SetShopWindow(ICollection<IShopItem> gallery)
        {
            var i = 0;
            foreach (var item in gallery)
            {
                var frame = itemFrames[i];
                i++;
                frame.Item.sprite = item.SpriteForShop;
                frame.Text.SetText($"{(item as INameableItem)!.Name}\nЗа {item.ItemCost} мусора");
                frame.Button.onClick.AddListener(() => OnButtonClick(frame, item));
            }
        }

        private void OnButtonClick(ShopItemFrame frame, IShopItem item)
        {
            //Реализация покупки предмета
            if (frame.Brought || !_garbageManager.IfCanAfford(item.ItemCost)) return;
            if (!_playerInventoryInput.HasSpace(item)) return;

            _garbageManager.GarbageBalance -= item.ItemCost;
            frame.OnBye();

            _playerInventoryInput.AddItem(item.SelfRef);

            frame.Text.SetText($"{(item as INameableItem)!.Name}\nПродано");
        }
    }
}