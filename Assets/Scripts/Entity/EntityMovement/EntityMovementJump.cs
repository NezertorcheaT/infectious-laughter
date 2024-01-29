using System;
using System.Collections;
using UnityEngine;

namespace Entity.EntityMovement
{
    public class EntityMovementJump : Ability
    {
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private float jumpHeight = 5f;
        [SerializeField] private float jumpTime = 1f;
        [SerializeField] private float groundDistance = 0.1f;
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
            StartCoroutine(ForceJump());
        }

        private IEnumerator ForceJump()
        {
            _rb.gravityScale = 0f;

            var initialYPos = _rb.position.y;
            float t;
            float prev = 0;
            Func<float, float> avFunc = ctx => jumpHeight * jumpCurve.Evaluate(ctx / jumpTime);

            for (t = 0; t < jumpTime; t += Time.fixedDeltaTime)
            {
                prev = (t - Time.fixedDeltaTime) / jumpTime;
                if (t > jumpTime / 5f &&
                    CheckGround(transform.position, _col, groundLayer, groundDistance, groundDistance))
                {
                    prev = (t - Time.fixedDeltaTime) / jumpTime;
                    break;
                }

                if (CheckTop(transform.position, _col, groundLayer, groundDistance, groundDistance))
                    t = t >= 0.5f * jumpTime ? t : jumpTime - t;

                _rb.position = new Vector2(_rb.position.x, initialYPos + avFunc(t));
                yield return new WaitForFixedUpdate();
            }

            _rb.velocity += new Vector2(0, (avFunc(t) - avFunc(prev)) / (t - prev));
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

        private static bool CheckTop(
            Vector3 worldPosition,
            Collider2D collider,
            LayerMask groundLayer,
            float groundDistance,
            float colliderOffset = 0.1f)
        {
            var checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            var checkPosition =
                worldPosition +
                new Vector3(0, collider.bounds.size.y / 2f) +
                (Vector3) collider.offset +
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