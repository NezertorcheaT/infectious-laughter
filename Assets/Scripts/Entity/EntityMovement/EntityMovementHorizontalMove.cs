using UnityEngine;

namespace Entity.EntityMovement
{
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField] private float speed;

        private Rigidbody2D _rb;

        public override void Initialize()
        {
            base.Initialize();
            _rb = GetComponent<Rigidbody2D>();
        }
        public void Move(float velocity)
        {
            if (!Available()) return;

            _rb.velocity = new Vector2(velocity * speed, _rb.velocity.y);
        }
    }
}