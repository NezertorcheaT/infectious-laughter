using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Tomb Of Reality Use Ability")]
    public class TombOfRealityUseAbility : Ability
    {
        private PropsImpact.TombOfReality _lastTomb;

        private bool _detectTombNow;
        public override void Initialize()
        {
            base.Initialize();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<PropsImpact.TombOfReality>()) return;
            Debug.Log("true");
            _lastTomb = other.gameObject.GetComponent<PropsImpact.TombOfReality>();
            Debug.Log(_lastTomb.gameObject.name);
            _detectTombNow = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<PropsImpact.TombOfReality>()) return;
            _detectTombNow = false;
            Debug.Log("false");
        }

        public void UseTombOfReality()
        {
            
            if (!_detectTombNow) return;
            _lastTomb.Use();
            Debug.Log("UseTomb");
        }
    }
}