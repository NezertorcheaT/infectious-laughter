using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Jump Ability")]
    public class EntityMovementJump : Ability
    {
        [SerializeField, CurveRange(0, 0, 1, 1)]
        private AnimationCurve jumpCurve;

        [SerializeField] private float jumpHeight = 5f;
        [SerializeField] private float jumpTime = 1f;
        [SerializeField] private float whenMax = 0.5f;
        [SerializeField] private float groundDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D _rb;
        private Collider2D _col;

        public float JumpTime => jumpTime;
        public float JumpHeight => jumpHeight;

        public override void Initialize()
        {
            base.Initialize();
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
        }


        public void Jump()
        {
            if (!Available()) return;
            if (!CheckGround(Entity.CachedTransform.position, Entity.CachedTransform.lossyScale, _col, groundLayer,
                0.1f)) return;
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
                while (!enabled)
                {
                    initialYPos = _rb.position.y - avFunc(t);
                    yield return new WaitForFixedUpdate();
                }

                prev = (t - Time.fixedDeltaTime) / jumpTime;
                if (t > jumpTime / 5f &&
                    CheckGround(Entity.CachedTransform.position, Entity.CachedTransform.lossyScale, _col, groundLayer,
                        groundDistance,
                        groundDistance))
                {
                    prev = (t - Time.fixedDeltaTime) / jumpTime;
                    break;
                }

                if (CheckTop(Entity.CachedTransform.position, Entity.CachedTransform.lossyScale, _col, groundLayer,
                    groundDistance,
                    groundDistance) && t < whenMax)
                {
                    var timesEquals = new List<float>(0);
                    for (var tt = 0f; tt < jumpTime; tt += Time.fixedDeltaTime)
                    {
                        if (Math.Abs(avFunc(tt) - avFunc(t)) < Time.fixedDeltaTime) timesEquals.Add(tt);
                    }

                    t = timesEquals.Last();
                }

                _rb.position = new Vector2(_rb.position.x, initialYPos + avFunc(t));
                yield return new WaitForFixedUpdate();
            }

            _rb.velocity += new Vector2(0, (avFunc(t) - avFunc(prev)) / (t - prev));
            _rb.gravityScale = 1f;
        }

        private static bool CheckGround(
            Vector3 worldPosition,
            Vector2 size,
            Collider2D collider,
            LayerMask groundLayer,
            float groundDistance,
            float colliderOffset = 0.1f)
        {
            var checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            var checkPosition =
                worldPosition +
                new Vector3(0, -collider.bounds.size.y / 2f) +
                (Vector3) collider.offset.Multiply(size) -
                new Vector3(0, groundDistance / 2f / size.y + colliderOffset);

            Helper.DrawBox(checkPosition, checkSize);
            return Physics2D.OverlapBoxAll(
                    checkPosition,
                    checkSize,
                    0,
                    groundLayer)
                .Length > 0;
        }

        private static bool CheckTop(
            Vector3 worldPosition,
            Vector2 size,
            Collider2D collider,
            LayerMask groundLayer,
            float groundDistance,
            float colliderOffset = 0.1f)
        {
            var checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            var checkPosition =
                worldPosition +
                new Vector3(0, collider.bounds.size.y / 2f) +
                (Vector3) collider.offset.Multiply(size) +
                new Vector3(0, groundDistance / 2f / size.y + colliderOffset);

            Helper.DrawBox(checkPosition, checkSize);
            return Physics2D.OverlapBoxAll(
                    checkPosition,
                    checkSize,
                    0,
                    groundLayer)
                .Length > 0;
        }
    }
}

public static class Helper
{
    public static Vector2 Multiply(this Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
    public static Vector3 Multiply(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vector2 Divide(this Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);
    public static Vector3 Divide(this Vector3 a, Vector3 b) => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);


    public static void DrawBox(Vector2 point, Vector2 size)
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