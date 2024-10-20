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
        [SerializeField, Min(0), Tooltip("в секундах")] private float frameShowDelay;
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

                frame.Item.sprite = slot.ShopItem.SpriteForShop;
                frame.Text.SetText($"{slot.LastItem.Name}\nЗа {slot.ShopItem.ItemCost} мусора");
                frame.Button.onClick.AddListener(() => OnButtonClick(frame, slot));
            }
        }

        private void OnButtonClick(ShopItemFrame frame, ISlot slot)
        {
            //Реализация покупки предмета
            if (!_garbageManager.IfCanAfford(slot.ShopItem.ItemCost) || slot.IsEmpty) return;
            if (!_playerInventoryInput.HasSpace(slot.LastItem)) return;

            _garbageManager.GarbageBalance -= slot.ShopItem.ItemCost;
            slot.Count = 0;

            _playerInventoryInput.AddItem(slot.LastItem.SelfRef);

            frame.Text.SetText($"{slot.LastItem.Name}\nПродано");
        }
    }
}