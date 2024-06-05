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

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            jumpCountActive = jumpCount;
        }

        private void OnCollisionEnter2D(Collision2D other)=>
            jumpCountActive = CheckGround() ? jumpCount : jumpCountActive;

        
        public void Jump()
        {
            if (jumpCountActive == 0) return;
            _playerRb.AddForce(new Vector2(_playerRb.velocity.x, jumpHeight), ForceMode2D.Impulse);
            jumpCountActive -= 1;
        }

        private bool CheckGround() =>
            Physics2D.Raycast(new Vector2(_startRayPoint.x + _playerRb.position.x, _startRayPoint.y + _playerRb.position.y), -_playerRb.transform.right, rayGroundRange).collider != null;
        private void Update()
        {
            Debug.DrawRay(new Vector2(_startRayPoint.x + _playerRb.position.x, _startRayPoint.y + _playerRb.position.y), -_playerRb.transform.right * rayGroundRange, Color.yellow);
        }
    
    /*
    private bool CheckGround()
    {
        RaycastHit2D CheckgroundHit = Physics2D.Raycast(new Vector2(_playerRb.position.x, _playerRb.position.y + rayOffsetY), -_playerRb.transform.up, rayGroundRange);
        if(CheckgroundHit.collider !=  null)
        {
            return true;
        }else{
            return false;
        }
    
    }
    */
}

}