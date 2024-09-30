using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Shop.Garbage
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;
        [Tooltip("in seconds")]
        [SerializeField, Min(0)] private float initialDelay = 1f;
        [Tooltip("in seconds")]
        [SerializeField, Min(0)] private float updateDelay = 0.25f;
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
            _garbageManager.OnBalanceChanged += OnBalanceChanged;
        }

        private void OnDisable()
        {
            _garbageManager.OnBalanceChanged -= OnBalanceChanged;
        }

        private void OnBalanceChanged(object sender, EventArgs e)
        {
            _currentBalance = _garbageManager.GarbageBalance;
            UpdateGarbageUI(_oldBalance, _currentBalance);
            _oldBalance = _currentBalance;
        }

        private async Task UpdateGarbageUI(int oldValue, int newValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(initialDelay));
            if (oldValue > newValue)
            {
                garbageText.text = _garbageManager.GarbageBalance.ToString();
                return;
            }

            var delay = TimeSpan.FromSeconds(updateDelay);
            for (var i = oldValue; i <= newValue; i++)
            {
                garbageText.text = i.ToString();
                await Task.Delay(delay);
            }
        }
    }
}