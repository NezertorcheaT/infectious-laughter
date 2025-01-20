using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CollideCheck))]
    [RequireComponent(typeof(HorizontalMovement))]
    [RequireComponent(typeof(Downing))]
    [AddComponentMenu("Entity/Abilities/Jump")]
    public class Jump : Ability, IJumpableAbility
    {
        [SerializeField, Min(0)] private int jumpCount;
        [SerializeField, Min(0)] public float jumpForce = 20;

        [Tooltip("влияет только на ии")] [SerializeField, Min(0)]
        private float jumpTime = 3;

        [Space(10)]
        [Tooltip("вектор, обозначающий силу толчка при прыжке от стены, всегда должен быть положительным")]
        [SerializeField, Min(0)]
        private Vector2 wallJumpPushForce = new(10, 20);

        [Tooltip("сколько вы не сможете двигаться после прыжка от стены")] [SerializeField, Min(0)]
        private float wallJumpDelay = 0.2f;

        private int _jumpCountActive;
        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private Downing _downing;
        private HorizontalMovement _movement;
        private bool _plm;
        private bool _pld;

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _movement = Entity.FindAbilityByType<HorizontalMovement>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
            _downing = Entity.FindExactAbilityByType<Downing>();
            _jumpCountActive = jumpCount;
        }

        private void TryJump()
        {
            if (!Available()) return;
            if (_collideCheck.IsTouchingGround || _jumpCountActive > 0) Perform();
            else if (_collideCheck.IsOnWall) _ = JumpFromWall();
        }

        void IJumpableAbility.Perform() => TryJump();
        float IJumpableAbility.JumpTime => jumpTime;

        private void Perform()
        {
            if (_collideCheck.IsTouchingGround)
                _jumpCountActive = jumpCount;
            else
                _jumpCountActive--;

            _playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            StartCoroutine(Stop());
            return;

            IEnumerator Stop()
            {
                yield return new WaitForSeconds(0.05f);
                _playerRb.velocity = new Vector2(0f, _playerRb.velocity.y);
            }
        }

        private async Task JumpFromWall()
        {
            if (_movement is null) return;
            if (_downing is null) return;
            if (_collideCheck is null) return;
            if (_playerRb is null) return;

            _plm = _movement.enabled;
            _pld = _downing.enabled;
            if (_pld) _downing.enabled = false;
            if (_plm) _movement.enabled = false;

            _playerRb.AddForce(
                new Vector2(
                    wallJumpPushForce.x * (_collideCheck.IsTouchingLeft ? 1 : _collideCheck.IsTouchingRight ? -1 : 0),
                    wallJumpPushForce.y
                ),
                ForceMode2D.Impulse
            );

            await UniTask.WaitForSeconds(wallJumpDelay);
            if (_movement is null) return;
            if (_downing is null) return;

            if (_pld) _downing.enabled = true;
            if (_plm) _movement.enabled = true;
        }
    }
}