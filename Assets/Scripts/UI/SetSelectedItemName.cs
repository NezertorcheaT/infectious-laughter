using UnityEngine;
using TMPro;
using Zenject;
using Installers;

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
            selfText.text = slot.IsEmpty
                ? "Empty"
                : slot.LastItem.Name;
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