using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Entity.Abilities{
public class GarbageAbility : Ability
{
    public int GarbageBalance { get; set; } = 0;

    [SerializeField] private TextMeshProUGUI _garbageText;
    private bool _garbageHasDetected;
    private int _detectedGarbageLevel;
    private GameObject _saveLastGarbage;

    [SerializeField, Min(1)] private int defaultGarbagePerLevel;

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
            if(_garbageHasDetected != true) return;
            GiveGarbage(_detectedGarbageLevel * defaultGarbagePerLevel);
            _saveLastGarbage.GetComponent<GarbageItem_>().Suicide();
            Debug.Log("Типа если тру");
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.GetComponent<GarbageItem_>())
            {
                _garbageHasDetected = true;
                _detectedGarbageLevel = other.gameObject.GetComponent<GarbageItem_>().Level;
                _saveLastGarbage = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other){
            if(other.gameObject.GetComponent<GarbageItem_>())
            {
                _garbageHasDetected = false;
            }
            }
            
        private void UpdateGarbageUI()
        {
            _garbageText.text = GarbageBalance.ToString();
        }

}
}