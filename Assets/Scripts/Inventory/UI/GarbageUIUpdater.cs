using Entity.Abilities;
using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class GarbageUIUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI garbageText;
        [SerializeField] private EntityGarbage garbage;

        private void OnEnable()
        {
            garbage.OnBalanceChanged += UpdateGarbageUI;
        }

        private void OnDisable()
        {
            garbage.OnBalanceChanged -= UpdateGarbageUI;
        }

        public void UpdateGarbageUI(int garbageCount) => garbageText.text = garbageCount.ToString();
    }
}