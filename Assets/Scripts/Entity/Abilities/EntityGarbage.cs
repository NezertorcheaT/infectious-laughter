using Inventory;
using Inventory.Garbage;
using UnityEngine;
using TMPro;

namespace Entity.Abilities
{
    public class EntityGarbage : Ability
    {
        public int GarbageBalance { get; private set; }

        [SerializeField] private TextMeshProUGUI _garbageText;
        [SerializeField, Min(1)] private int defaultGarbagePerLevel;

        private bool _garbageHasDetected;
        private int _detectedGarbageLevel;
        private GameObject _saveLastGarbage;

        public void Start()
        {
            GiveGarbage(10);
        }

        public void GiveGarbage(int GiveGarbageBalance)
        {
            GarbageBalance += GiveGarbageBalance;
            UpdateGarbageUI();
        }

        public void PickGarbage()
        {
            if (_garbageHasDetected != true) return;
            GiveGarbage(_detectedGarbageLevel * defaultGarbagePerLevel);
            _saveLastGarbage.GetComponent<GarbageItem>().Suicide();
            Debug.Log("Типа если тру");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<GarbageItem>()) return;
            _garbageHasDetected = true;
            _detectedGarbageLevel = other.gameObject.GetComponent<GarbageItem>().Level;
            _saveLastGarbage = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<GarbageItem>()) return;
            _garbageHasDetected = false;
        }

        private void UpdateGarbageUI()
        {
            _garbageText.text = GarbageBalance.ToString();
        }
    }
}