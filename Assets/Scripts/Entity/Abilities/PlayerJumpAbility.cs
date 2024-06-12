using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Jump Ability")]
    public class PlayerJumpAbility : Ability
    {
        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [Space(10f)] [SerializeField] private float jumpHeight = 3;
        [Space(10f)][SerializeField] private float walljumpHeight;
        [Space(10f)][SerializeField] private float walljumpPush;

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

        public void TryToJump()
        {
            if (_collideCheck.IsTouchingGround) Jump();
            else if (_collideCheck.TestOnWall()) JumpFromWall();
        }

        public void Jump()
        {
            _jumpCountActive = jumpCount;
            if (_jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            _jumpCountActive -= 1;
        }

        public void JumpFromWall()
        {
            _playerRb.AddForce(new Vector2(walljumpPush * GetTrajectory(), walljumpHeight), ForceMode2D.Impulse);
            Debug.Log(_playerRb.velocity);
        }

        private int GetTrajectory()
        {
            int answ = 0;
            if (_collideCheck.IsTouchingLeft) answ = -1;
            else if (_collideCheck.IsTouchingRight) answ = 1;
            return -answ;
        }
    }
}