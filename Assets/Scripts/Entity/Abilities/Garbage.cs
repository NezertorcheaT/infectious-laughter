using Shop;
using Shop.Garbage;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Garbage")]
    public class Garbage : Ability
    {
        [SerializeField, Min(1)] private int defaultGarbagePerLevel;
        [Inject] private GarbageManager _garbageManager;

        private bool _garbageHasDetected;
        private int _detectedGarbageLevel;
        private GameObject _saveLastGarbage;

        public void PickGarbage()
        {
            if (_garbageHasDetected != true) return;
            _garbageManager.GarbageBalance += _detectedGarbageLevel * defaultGarbagePerLevel;
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