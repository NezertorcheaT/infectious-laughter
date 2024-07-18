using System;
using Shop;
using TMPro;
using UnityEngine;
using Zenject;

namespace Inventory.UI
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;
        [Inject] private GarbageManager _garbageManager;

        private void Start()
        {
            _garbageManager.OnBalanceChanged += GarbageBalance_OnBalanceChanged;

            UpdateGarbageUI();
        }

        private void GarbageBalance_OnBalanceChanged(object sender, EventArgs e)
        {
            UpdateGarbageUI();
        }

        //Сделать через ивент обновление
        private void UpdateGarbageUI() => garbageText.text = _garbageManager.GarbageBalance.ToString();
    }
}