using UnityEngine;

namespace Entity.Abilities
{
    public class PlayerJumpAbility : Ability
    {
        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [SerializeField] private float jumpHeight = 3;
        [SerializeField] private float groundDistance = 3;

        private Rigidbody2D _playerRb;
        private int jumpCountActive;

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            jumpCountActive = jumpCount;
        }

        private void OnCollisionEnter2D(Collision2D other) =>
            jumpCountActive = CheckGround() ? jumpCount : jumpCountActive;

        public void Jump()
        {
            if (jumpCountActive == 0) return;
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, jumpHeight);
            jumpCountActive -= 1;
        }

        private bool CheckGround() =>
            Physics2D.Raycast(_playerRb.position, -_playerRb.transform.up, groundDistance).collider != null;
    }
}