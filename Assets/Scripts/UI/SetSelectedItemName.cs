using UnityEngine;
using TMPro;
using Zenject;
using Installers;
using Inventory;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SetSelectedItemName : MonoBehaviour
    {
        [Inject] private PlayerInstallation _playerInstallation;
        [SerializeField] private TMP_Text selfText;
        [SerializeField] private Inventory.UI.InventoryUI playerInventoryUI;

        private void SetItemName(int selectionId)
        {
            var slot = _playerInstallation.Inventory.Slots[selectionId];
            var item = slot.LastItem as INameableItem;
            selfText.text = slot.IsEmpty
                ? "Empty"
                : item.Name;
        }

        private void OnEnable()
        {
            selfText ??= gameObject.GetComponent<TMP_Text>();
            playerInventoryUI.OnSelect += SetItemName;
        }

        private void OnDisable()
        {
            playerInventoryUI.OnSelect -= SetItemName;
        }
    }
}