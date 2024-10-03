using System;
using PropsImpact;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Light Responsive")]
    public class LightResponsive : Ability
    {
        public bool InLight { get; private set; }
        public event Action<LightImpact> OnEnterLight;
        public event Action<LightImpact> OnExitLight;

        public bool Resistance;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(Resistance) return;
            if (!other.GetComponent<LightImpact>()) return;
            InLight = true;
            OnEnterLight?.Invoke(other.GetComponent<LightImpact>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(Resistance)
            {
                if(!other.GetComponent<LightImpact>()) return;
                InLight = false;
                Resistance = false;
                return;
            }
            
            if (!other.GetComponent<LightImpact>()) return;
            InLight = false;
            Resistance = false;
            OnExitLight?.Invoke(other.GetComponent<LightImpact>());
        }
    }
}