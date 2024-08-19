using System;
using Entity.Abilities;
using UnityEngine;

namespace PropsImpact
{
    public class TrapImpact : MonoBehaviour
    {
        [SerializeField] private float stunTime = 5;
        public event Action OnTrapClosed;

        private async void OnCollisionEnter2D(Collision2D other)
        {
            var ability = other.collider.gameObject.GetComponent<Stun>();
            if (ability is null) return;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<Inventory.ItemToAdd>().enabled = false;
            OnTrapClosed?.Invoke();
            await ability.Perform(stunTime);
            Destroy(gameObject);
        }
    }
}