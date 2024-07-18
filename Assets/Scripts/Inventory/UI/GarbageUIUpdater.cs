using System;
using System.Collections;
using Entity.Abilities;
using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;

        private void Start ()
        {
            GarbageManager.Instance.OnBalanceChanged += GarbageBalance_OnBalanceChanged;

            UpdateGarbageUI();
        }

        private void GarbageBalance_OnBalanceChanged(object sender, System.EventArgs e)
        {
            UpdateGarbageUI();
        }

        //Сделать через ивент обновление
        private void UpdateGarbageUI() => garbageText.text = GarbageManager.Instance.GetGarbageBalance().ToString();
    }
}