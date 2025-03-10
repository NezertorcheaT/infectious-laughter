using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(HorizontalMovement))]
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Dash")]
    public class Dash : Ability
    {
        [SerializeField] private HorizontalMovement playerMovement;
        [SerializeField, Min(0)] private float dashForce;
        [SerializeField, Min(0)] private float dashMovementDelay = 0.5f;
        [SerializeField, Min(0)] private float dashCooldown;
        [SerializeField, Min(1)] private int dashCount;
        private int _currentDashCount;
        private Rigidbody2D _rb;
        private bool _plm;

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _currentDashCount = dashCount;
        }

        private async Task DisableMovement()
        {
            await UniTask.WaitForSeconds(dashMovementDelay);
            playerMovement.enabled = _plm;
        }

        public async Task Perform()
        {
            if (!Available()) return;
            if (_currentDashCount <= 0) return;

            _plm = playerMovement.enabled;
            playerMovement.enabled = false;
            _rb.velocity = new Vector2(dashForce * (playerMovement.Turn ? 1f : -1f), 0);
            _currentDashCount--;
            if (_plm) _ = DisableMovement();
            else playerMovement.enabled = _plm;
            await UniTask.WaitForSeconds(dashCooldown * dashCount);
            _currentDashCount = dashCount;
        }
    }
}