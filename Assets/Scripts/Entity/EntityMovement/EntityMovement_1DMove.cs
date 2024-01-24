using UnityEngine;

namespace Scripts.Entity
{
    public class EntityMovement_1DMove : Ability
    {
        [SerializeField] private float speed;

        private Rigidbody2D rb;

        public override void Initialize()
        {
            base.Initialize();
            rb = GetComponent<Rigidbody2D>();
        }
        public void Move(float velocity)
        {
            if (!Available()) return;

            rb.velocity = new Vector2(velocity * speed, rb.velocity.y);
        }
    }
}