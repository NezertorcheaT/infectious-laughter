using System;
using System.Threading.Tasks;
using Installers;
using Inventory;
using Inventory.Input;
using UnityEngine;
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
        [Inject] private PlayerInstallation _playerInstallation;
        [Inject] private GarbageManager _garbageManager;
        private PlayerInventoryInput _playerInventoryInput;

        private void Start()
        {
            _playerInventoryInput = _playerInstallation.Entity.FindAbilityByType<PlayerInventoryInput>();
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
        }

        public async void OpenShop()
        {
            await Task.Delay(TimeSpan.FromSeconds(frameShowDelay));
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