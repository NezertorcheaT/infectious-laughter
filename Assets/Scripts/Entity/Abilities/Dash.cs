using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Dash")]
    public class Dash : Ability
    {
        [SerializeField] private HorizontalMovement playerMovement;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldown;
        [SerializeField] private int dashCount;
        private int _currentDashCount;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _currentDashCount = dashCount;
        }

        public async Task Perform()
        {
            if (!Available()) return;
            if (_currentDashCount <= 0) return;

            _rb.velocity = new Vector2(dashForce * (playerMovement.Turn ? 1f : -1f), 0);
            _currentDashCount--;
            await UniTask.WaitForSeconds(dashCooldown * dashCount);
            _currentDashCount = dashCount;
        }
    }
}