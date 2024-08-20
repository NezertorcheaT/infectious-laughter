using Installers;
using Inventory;
using Inventory.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Transform buttonTemplate;
        [SerializeField] private Transform shopUITransform;
        [Inject] private PlayerInstallation _playerInstallation;
        [Inject] private GarbageManager _garbageManager;
        private PlayerInventoryInput _playerInventoryInput;

        private void Awake()
        {
            buttonTemplate.gameObject.SetActive(false);
            _playerInventoryInput = _playerInstallation.Entity.FindAbilityByType<PlayerInventoryInput>();
        }

        public void SetItemSO(IInventory shopInventory)
        {
            foreach (ISlot slotInventory in shopInventory.Slots)
            {
                Transform slot = Instantiate(buttonTemplate, shopUITransform);
                slot.gameObject.GetComponentInChildren<Image>().sprite = slotInventory.LastItem.Sprite;
                slot.gameObject.GetComponentInChildren<TextMeshProUGUI>()
                    .SetText(slotInventory.LastItem.ItemCost.ToString());

                slot.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    OnButtonClick(slot, slotInventory);
                });

                buttonTemplate.gameObject.SetActive(true);
            }
        }

        private void OnButtonClick(Transform slot, ISlot slotInventory)
        {
            //Реализация покупки предмета
            if (!_garbageManager.IfCanAfford(slotInventory.LastItem.ItemCost) || slotInventory.IsEmpty) return;
            if (!_playerInventoryInput.HasSpace(slotInventory.LastItem)) return;

            _garbageManager.GarbageBalance -= slotInventory.LastItem.ItemCost;
            slotInventory.Count = 0;

            _playerInventoryInput.AddItem(slotInventory.LastItem.SelfRef);

            slot.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("SOLD");
        }
    }
}