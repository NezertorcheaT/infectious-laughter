using UnityEngine;

namespace Entity.Abilities
{
    public class PlayerJumpAbility : Ability
    {
        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [Space(10f)] [SerializeField] private float jumpHeight = 3;
        [SerializeField] private int maxSlopeAngle;

        private Rigidbody2D _playerRb;
        private int _jumpCountActive;
        private bool _сanJumpCountRecover;

        public bool OnGround { get; private set; }

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _jumpCountActive = jumpCount;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            for (var i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(Vector2.up, collision.contacts[i].normal) >= maxSlopeAngle) continue;
                if (!_сanJumpCountRecover) continue;
                _jumpCountActive = jumpCount;
                _сanJumpCountRecover = false;
                OnGround = true;
                return;
            }

            OnGround = false;
        }

        private void OnCollisionExit2D(Collision2D other) => _сanJumpCountRecover = true;

        public void Jump()
        {
            if (_jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            _jumpCountActive -= 1;
        } 
        /*private bool CheckGround()
        {
            RaycastHit2D CheckgroundHit = Physics2D.Raycast(new Vector2(_playerRb.position.x, _playerRb.position.y + rayOffsetY), -_playerRb.transform.up, rayGroundRange);
            if(CheckgroundHit.collider !=  null)
            {
                return true;
            }else{
                return false;
            }
        
        }*/
    }
}