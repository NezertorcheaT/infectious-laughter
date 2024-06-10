using UnityEngine;

namespace Entity.Abilities
{
public class PlayerJumpAbility : Ability
{

    //private Rigidbody2D _playerRb;


    //private int jumpCountActive;

    //[Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
    //[SerializeField] private float jumpHeight = 3;



        [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
        [Space(10f)]
        [SerializeField, Min(-2f)] private float rayOffsetY;

        [SerializeField] private float jumpHeight = 3;
        private float rayGroundRange = 0.9f; // Не изменять, идеяльно находится под игроком
        private Vector2 _startRayPoint = new Vector2(0.45f, -1.5f); // Не изменять, ровно под игроком

        private Rigidbody2D _playerRb;
        private int jumpCountActive;
        [SerializeField]private int maxSlopeAngle;
        private bool сanJumpCountRecover;
        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            jumpCountActive = jumpCount;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(Vector2.up, collision.contacts[i].normal) < maxSlopeAngle)
                {
                    if(!сanJumpCountRecover) continue;
                    jumpCountActive = jumpCount;
                    сanJumpCountRecover = false;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other) => сanJumpCountRecover = true;

        
        public void Jump()
        {
            if (jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            jumpCountActive -= 1;
        }
}

}