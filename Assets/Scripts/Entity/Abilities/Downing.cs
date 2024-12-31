using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Entity.Abilities.DowningHelper;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CollideCheck))]
    [AddComponentMenu("Entity/Abilities/Downing")]
    public class Downing : Ability
    {
        [Tooltip("скорость, с которой будет сползать")] [SerializeField]
        private float speed;

        [Tooltip("сколько ждать, перед тем как начать сползать")] [SerializeField]
        private float downingTime;

        private Rigidbody2D _rb;
        private CollideCheck _collideCheck;
        private bool _getToWall;
        private bool _isDowning;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
        }

        public void WallDowning(float playerInput)
        {
            var downingActive = 0f;
            var velX = _rb.velocity.x;
            var trajectory = _collideCheck.GetTrajectory(playerInput);

            if (
                !Available() ||
                !(trajectory != 0 || _getToWall) ||
                _collideCheck.IsTouchingGround
            ) return;

            if (!_getToWall)
            {
                _getToWall = true;
                _isDowning = false;
                _ = WaitDowningDelay();
            }

            if (_isDowning) downingActive = -speed;
            if (trajectory != 0) velX = 0;
            _rb.velocity = new Vector2(velX, downingActive);

            if (!_collideCheck.IsOnWall) _getToWall = false;
        }

        private async Task WaitDowningDelay()
        {
            await UniTask.WaitForSeconds(downingTime);
            _isDowning = true;
        }
    }

    namespace DowningHelper
    {
        public static class DowningExtensions
        {
            public static int GetTrajectory(this CollideCheck collideCheck, float inputVelocity) =>
                collideCheck.IsTouchingLeft && inputVelocity < 0
                    ? -1
                    : collideCheck.IsTouchingRight && inputVelocity > 0
                        ? 1
                        : 0;
        }
    }
}