using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Jump Ability")]
    public class PlayerJumpAbility : Ability
    {
        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [Space(10f)] [SerializeField] private float jumpHeight = 3;

        private Rigidbody2D _playerRb;
        private int _jumpCountActive;
        private GroundCheck _groundCheck;


        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _groundCheck = Entity.FindAbilityByType<GroundCheck>();
            _jumpCountActive = jumpCount;
        }

        public void Jump()
        {
            if (_groundCheck && _groundCheck.IsTouchingGround) _jumpCountActive = jumpCount;
            if (_jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            _jumpCountActive -= 1;
        }
    }
}