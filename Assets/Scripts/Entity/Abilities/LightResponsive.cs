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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<LightImpact>()) return;
            InLight = true;
            OnEnterLight?.Invoke(other.GetComponent<LightImpact>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<LightImpact>()) return;
            InLight = false;
            OnExitLight?.Invoke(other.GetComponent<LightImpact>());
        }
    }
}