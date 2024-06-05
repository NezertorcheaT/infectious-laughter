using Inventory;
using Inventory.Garbage;
using UnityEngine;
using System.Collections;
using TMPro;

namespace Entity.Abilities
{
    public class EntityGarbage : Ability
    {
        public int GarbageBalance { get; private set; }

        private bool _garbageHasDetected;
        [SerializeField, Min(1)] private int defaultGarbagePerLevel;
        private int _detectedGarbageLevel;
        private GameObject _saveLastGarbage;
        private GarbageUIUpdater _uiUpdater;
        [SerializeField]private float _oneMoneyColdown;

        public void Start()
        {
            _uiUpdater = GetComponent<GarbageUIUpdater>();
            StartCoroutine(GiveGarbage(10));
            _uiUpdater.UpdateGarbageUI();
        }

        public IEnumerator GiveGarbage(int GiveGarbageBalance)
        {
            int givedGarbage = 0;
            while(GiveGarbageBalance != givedGarbage)
            {
            yield return new WaitForSeconds(_oneMoneyColdown);
            GarbageBalance++;
            givedGarbage++;
            _uiUpdater.UpdateGarbageUI();
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