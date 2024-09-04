using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    public class BlessingPlaceUsing : Ability
    {
        private bool _canUse;
        private PropsImpact.BlessingPlace _lastBlessingPlace;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.GetComponent<PropsImpact.BlessingPlace>()) return;
            _canUse = true;
            _lastBlessingPlace = other.gameObject.GetComponent<PropsImpact.BlessingPlace>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.gameObject.GetComponent<PropsImpact.BlessingPlace>()) return;
            _canUse = false;
        }

        public void TryUseBlessingPlace()
        {
            if(!_canUse) return;
            _lastBlessingPlace.UseBlessingPlace();
            _canUse = false;
        }
    }
}
