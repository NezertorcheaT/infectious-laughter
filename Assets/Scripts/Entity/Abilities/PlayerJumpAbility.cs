using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Jump Ability")]
    public class PlayerJumpAbility : Ability
    {
        [SerializeField, Min(1)] private int jumpCount = 1;
        [SerializeField, Min(0)] private float jumpHeight = 3;
        [Space(10)]
        [SerializeField, Min(0)] private float walljumpHeight;
        [SerializeField, Min(0)] private float walljumpPush;

        private int _jumpCountActive;

        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private EntityMovementDowning _movementDowning;


        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
            _movementDowning = Entity.FindAbilityByType<EntityMovementDowning>();
            _jumpCountActive = jumpCount;
        }

        public void TryJump()
        {
            if (_collideCheck.IsTouchingGround) Jump();
            else if (_collideCheck.IsOnWall) JumpFromWall();
        }

        private void Jump()
        {
            _jumpCountActive = jumpCount;
            if (_jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            _jumpCountActive -= 1;
        }

        private void JumpFromWall()
        {
            _playerRb.AddForce(
                new Vector2(
                    walljumpPush * (_collideCheck.IsTouchingLeft ? 1 : _collideCheck.IsTouchingRight ? 1 : 0),
                    walljumpHeight
                ),
                ForceMode2D.Impulse
            );
        }
    }
}