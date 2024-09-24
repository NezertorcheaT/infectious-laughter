using System;
using System.Threading.Tasks;
using Shop;
using TMPro;
using UnityEngine;
using Zenject;

namespace Inventory.UI
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;
        [Range(0, 500f)]
        [SerializeField] private float UpdateDelayInMilliseconds = 250;
        [Inject] private GarbageManager _garbageManager;
        private int _oldBalance;
        private int _currentBalance;
        private void Start()
        {
            _oldBalance = _garbageManager.GarbageBalance;

            UpdateGarbageUI(_oldBalance, _currentBalance);
        }

        private void OnEnable()
        {
            _garbageManager.OnBalanceChanged += GarbageBalance_OnBalanceChanged;
        }

        private void GarbageBalance_OnBalanceChanged(object sender, EventArgs e)
        {
            _currentBalance = _garbageManager.GarbageBalance;
            UpdateGarbageUI(_oldBalance, _currentBalance);
            _oldBalance = _currentBalance;
        }

        private void GarbageBalance_BalanceChanged(object sender, EventArgs e)
        {
            Debug.Log(sender.GetType());
        }

        //Сделать через ивент обновление

        private async void UpdateGarbageUI(int oldValue, int newValue)
        {
            if(oldValue > newValue) garbageText.text = _garbageManager.GarbageBalance.ToString();
            else
            {
                for(int i = oldValue; i <= newValue; i++)
                {
                    garbageText.text = i.ToString();
                    await Task.Delay((int)UpdateDelayInMilliseconds);
                }
            }
        }

        private void OnDisable()
        {
            _garbageManager.OnBalanceChanged -= GarbageBalance_OnBalanceChanged;
        }
    }
}