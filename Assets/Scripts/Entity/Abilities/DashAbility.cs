using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Dash Ability")]
    public class DashAbility : Ability
    {
        [SerializeField] private EntityMovementHorizontalMove playerMovement;
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

        public async void Dash()
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