using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Installers;
using UnityEngine.InputSystem;

namespace UI
{
    public class SetSelectedItemName : MonoBehaviour
    {
        private TMP_Text _selfText;
        private Inventory.UI.InventoryUI _playerInventoryUI;

        [Inject] private MainCanvasInstaller.MainCanvasInstallation _canvasInstallation;
        [Inject] private PlayerInstallation _playerInstallation;
        

        public void SetItemName(int selectionId)
        {
            if(_playerInstallation.Inventory.Slots[selectionId].IsEmpty)
            {
                _selfText.text = "Empty";
            }
            else
            {
                _selfText.text = _playerInstallation.Inventory.Slots[selectionId].LastItem.Name;
            }
        }

        private void Start()
        {
            _selfText = gameObject.GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _playerInventoryUI = _canvasInstallation.CanvasObject.transform.GetChild(0).GetChild(0).GetComponent<Inventory.UI.InventoryUI>();
            _playerInventoryUI.OnSelect += SetItemName;
        }

        private void OnDisable()
        {
            _playerInventoryUI.OnSelect -= SetItemName;
        }


    }
}