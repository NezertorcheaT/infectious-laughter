using Entity.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entity.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Ground Check")]
    public class GroundCheck : Ability
    {
        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [Space(10f)][SerializeField] private float jumpHeight = 3;
        [SerializeField] private int maxSlopeAngle;

        private int _jumpCountActive;
        private bool _�anJumpCountRecover;

        bool OnGround = false;

        private void Start()
        {
            _jumpCountActive = jumpCount;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            for (var i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(Vector2.up, collision.contacts[i].normal) >= maxSlopeAngle) continue;
                if (!_�anJumpCountRecover) continue;
                _jumpCountActive = jumpCount;
                _�anJumpCountRecover = false;
                OnGround = true;
                return;
            }

            OnGround = false;
        }

        private void OnCollisionExit2D(Collision2D other) => _�anJumpCountRecover = true;

        public int JumpCounter()
        {
            return _jumpCountActive;
        }
    }
}
