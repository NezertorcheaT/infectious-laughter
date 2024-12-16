using System.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CollideCheck))]
    [RequireComponent(typeof(Downing))]
    [AddComponentMenu("Entity/Abilities/Jump")]
    public class Jump : Ability, IJumpableAbility
    {
        [SerializeField, Min(1)] private int jumpCount = 1;
        [SerializeField, Min(0)] public float jumpForce = 20;
        [SerializeField, Min(0)] private float jumpTime = 3;
        [Space(10)] [SerializeField, Min(0)] private float wallJumpHeight = 10;
        [SerializeField, Min(0)] private float wallJumpPush = 20;
        private int _jumpCountActive;
        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private Downing _movementDowning;

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
            _movementDowning = Entity.FindExactAbilityByType<Downing>();
            _jumpCountActive = jumpCount;
        }

        private void TryJump()
        {
            if (_collideCheck.IsTouchingGround) Perform();
            else if (_collideCheck.IsOnWall) JumpFromWall();
        }

        void IJumpableAbility.Perform() => TryJump();
        float IJumpableAbility.JumpTime => jumpTime;

        private void Perform()
        {
            _jumpCountActive = jumpCount;
            if (_jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpForce), ForceMode2D.Impulse);

            _jumpCountActive -= 1;
        }

        private async void JumpFromWall()
        {
            _movementDowning.enabled = false;
            _playerRb.AddForce(
                new Vector2(
                    wallJumpPush * (_collideCheck.IsTouchingLeft ? 1 : _collideCheck.IsTouchingRight ? -1 : 0),
                    wallJumpHeight
                ),
                ForceMode2D.Impulse
            );
            await Task.Delay((int)(jumpTime / 2f * 1000f));
            _movementDowning.enabled = true;
        }
    }
}