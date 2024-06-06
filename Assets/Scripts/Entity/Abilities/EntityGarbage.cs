using System;
using Inventory.Garbage;
using UnityEngine;
using System.Collections;

namespace Entity.Abilities
{
    public class EntityGarbage : Ability
    {
        [SerializeField, Min(1)] private int defaultGarbagePerLevel;
        [SerializeField] private float oneMoneyColdown = .05f;

        private bool _garbageHasDetected;
        private int _detectedGarbageLevel;
        private GameObject _saveLastGarbage;
        private int _garbageBalance;
        
        public event Action<int> OnBalanceChanged;

        public void Start()
        {
            StartCoroutine(GiveGarbage(10));
        }

        public IEnumerator GiveGarbage(int GiveGarbageBalance)
        {
            var givedGarbage = 0;
            while (GiveGarbageBalance != givedGarbage)
            {
                yield return new WaitForSeconds(oneMoneyColdown);
                _garbageBalance++;
                givedGarbage++;
                OnBalanceChanged?.Invoke(_garbageBalance);
            }
        }

        public void PickGarbage()
        {
            if (_garbageHasDetected != true) return;
            StartCoroutine(GiveGarbage(_detectedGarbageLevel * defaultGarbagePerLevel));
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