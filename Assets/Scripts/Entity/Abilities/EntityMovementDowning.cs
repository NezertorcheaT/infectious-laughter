using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Downing Ability")]
    public class EntityMovementDowning : Ability
    {
        [SerializeField] private float speed;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void WallDowning(float velocity, bool onWall)
        {
            //if () return;

            _rb.velocity = new Vector2(0, -speed);
        }

        public void JumpFromWall()
        {

        }
    }
}