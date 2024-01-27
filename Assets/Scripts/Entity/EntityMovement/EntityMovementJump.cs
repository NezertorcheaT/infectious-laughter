using System.Threading.Tasks;
using Scripts.Entity;
using UnityEngine;

namespace Entity.EntityMovement
{
    public class EntityMovementJump : Ability
    {
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float jumpTime = 1f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D _rb;
        private Collider2D _col;

        public override void Initialize()
        {
            base.Initialize();
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
        }


        public void Jump()
        {
            if (!Available()) return;
            if (!CheckGround(transform.position, _col, groundLayer, 0.1f)) return;
            ForceJump();
        }

        private async void ForceJump()
        {
            _rb.gravityScale = 0f;

            for (float t = 0; t < jumpTime; t += Time.deltaTime)
            {
                if (t > jumpTime / 5f && CheckGround(transform.position, _col, groundLayer, 0.1f)) break;
                _rb.velocity = new Vector2(_rb.velocity.x, jumpCurve.Evaluate(t / jumpTime) * jumpForce);
                await Task.Yield();
            }

            _rb.gravityScale = 1f;
        }

        private static bool CheckGround(
            Vector3 worldPosition,
            Collider2D collider,
            LayerMask groundLayer,
            float groundDistance,
            float colliderOffset = 0.1f)
        {
            var checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            var checkPosition =
                worldPosition +
                new Vector3(0, -collider.bounds.size.y / 2f) +
                (Vector3) collider.offset -
                new Vector3(0, groundDistance / 2f + colliderOffset);

            DrawBox(checkPosition, checkSize);

            return Physics2D.OverlapBoxAll(
                    checkPosition,
                    checkSize,
                    0,
                    groundLayer)
                .Length > 0;
        }

        private static void DrawBox(Vector2 point, Vector2 size)
        {
            Debug.DrawLine(point + new Vector2(size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, size.y / 2f));
        }
    }
}