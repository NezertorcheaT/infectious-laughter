using System;
using System.Threading.Tasks;
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
        [Inject] private PlayerInstallation _playerInstallation;
        [Inject] private GarbageManager _garbageManager;
        [Inject] private Controls _controls;
        private PlayerInventoryInput _playerInventoryInput;
        private InputAction _shopClosingAction;
        private bool _isShopClosed { get; set; }

        private void Start()
        {
            _playerInventoryInput = _playerInstallation.Entity.FindAbilityByType<PlayerInventoryInput>();
            _shopClosingAction = _controls.FindAction("CloseShop");
            backPanel.SetActive(false);
        }

        private void Update()
        {
              if ((!_isShopClosed) && _shopClosingAction.WasPerformedThisFrame()) CloseShop();
        }

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
            _isShopClosed = true;
            backPanel.SetActive(false);
        }

        public async void OpenShop()
        {
            backPanel.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(frameShowDelay));
            background.enabled = true;

            foreach (var frame in itemFrames)
            {
                frame.gameObject.SetActive(true);
                frame.Animate();
            }
            _isShopClosed = false;
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