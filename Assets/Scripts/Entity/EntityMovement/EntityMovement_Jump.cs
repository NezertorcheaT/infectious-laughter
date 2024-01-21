using UnityEngine;

namespace Scripts.Entity
{
    public class EntityMovement_Jump : Ability
    {
        [SerializeField] private float force;
        [SerializeField] private Vector2 groundRaycastOffset = new Vector2();

        private Rigidbody2D rb;
        private Collider2D col;

        public override void Initialize()
        {
            base.Initialize();
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }
        public void Jump()
        {
            Vector2 transformWithOffset = transform.position + new Vector3(groundRaycastOffset.x, groundRaycastOffset.y, 0.0f);
            if (Physics2D.OverlapBox(
                new Vector2(transformWithOffset.x, transformWithOffset.y - 0.025f), // Center
                new Vector2(col.bounds.size.x, 0.0015f), 0.0f) != null)             // Size
            {
                rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
        }
    }
}