using System.Collections;
using Entity.Abilities;
using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;
        [SerializeField] private EntityGarbage garbage;
        [SerializeField] private float oneMoneyCooldown = .05f;
        private int _textGarbage;
        private int _needGarbage;
        private IEnumerator _garbageCoroutine;

        private void OnEnable()
        {
            _textGarbage = garbage.GarbageBalance;
            _needGarbage = garbage.GarbageBalance;
            garbage.OnBalanceChanged += AddGarbage;
            UpdateGarbageUI(_textGarbage);
            _garbageCoroutine = GiveGarbage();
            StartCoroutine(_garbageCoroutine);
        }

        private void OnDisable()
        {
            garbage.OnBalanceChanged -= AddGarbage;
            if (_garbageCoroutine is not null)
                StopCoroutine(_garbageCoroutine);
        }

        private IEnumerator GiveGarbage()
        {
            while (true)
            {
                yield return new WaitForSeconds(oneMoneyCooldown);
                if (_needGarbage == 0) continue;
                if (_needGarbage > 0)
                {
                    _textGarbage++;
                    _needGarbage--;
                }
                else
                {
                    _textGarbage--;
                    _needGarbage++;
                }

                UpdateGarbageUI(_textGarbage);
            }
        }

        private void AddGarbage(int garbageCount) => _needGarbage = garbageCount;

        private void UpdateGarbageUI(int garbageCount) => garbageText.text = garbageCount.ToString();
    }
}