using System;
using Inventory.Garbage;
using UnityEngine;

namespace Entity.Abilities
{
    public class EntityGarbage : Ability
    {
        [SerializeField, Min(1)] private int defaultGarbagePerLevel;

        private bool _garbageHasDetected;
        private int _detectedGarbageLevel;
        private GameObject _saveLastGarbage;

        public int GarbageBalance { get; private set; }
        public event Action<int> OnBalanceChanged;

        public void PickGarbage()
        {
            if (_garbageHasDetected != true) return;
            GarbageBalance += _detectedGarbageLevel * defaultGarbagePerLevel;
            OnBalanceChanged?.Invoke(_detectedGarbageLevel * defaultGarbagePerLevel);
            _saveLastGarbage.GetComponent<GarbageItem>().Suicide();
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
    }
}