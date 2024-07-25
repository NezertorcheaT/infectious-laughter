using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Alt Jump Ability")]
    public class EntityMovementJump : Ability, IJumpableAbility
    {
        [SerializeField, CurveRange(0, 0, 1, 1)]
        private AnimationCurve jumpCurve;

        [Space(10.0f)] [SerializeField] private float jumpHeight = 5f;

        [SerializeField, Min(0.69f), Tooltip("Значения меньше 0.69 могут обосрать платформы")]
        private float jumpTime = 1f;

        [
            SerializeField,
            Tooltip("Выставьте значение от 0 до 1, при котором на графике находится максимальная точка"),
            Range(0f, 1f)
        ]
        private float whenMax = 0.5f;

        [Space(10.0f), SerializeField, Min(0.01f)]
        private float groundDistance = 0.1f;

        [SerializeField] private LayerMask groundLayer;
        [Space(10.0f), SerializeField, Min(1)] private int jumpsCount = 1;

        private int curJumpsCount;

        private Rigidbody2D _rb;
        private Collider2D _col;
        private CollideCheck _collideCheck;

        public float CurrentJumpTime { get; private set; }
        public float JumpHeight => jumpHeight;
        public float WhenMax => whenMax;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
            _collideCheck = Entity.FindExactAbilityByType<CollideCheck>();

            curJumpsCount = jumpsCount;
        }

        public async UniTask Jump(bool force = false)
        {
            if (!Available()) return;
            if (curJumpsCount == 0 && !force)
            {
                if (!_collideCheck.IsTouchingGround) return;
                curJumpsCount = jumpsCount;
            }

            curJumpsCount--;
            await StopJumps();
            await ForceJump();
        }

        async void IJumpableAbility.Jump() => await Jump();
        float IJumpableAbility.JumpTime => jumpTime;

        private bool _jumping = false;
        private bool _stopAllJumps = false;

        public async UniTask StopJumps()
        {
            _stopAllJumps = true;
            await UniTask.WaitForFixedUpdate();
            _stopAllJumps = false;
            _jumping = false;
        }

        public async UniTask DropRigidBody(float t)
        {
            await ForceJump(t / jumpTime);
        }

        /// <summary>
        /// initialTime от 0 до 1
        /// </summary>
        /// <param name="initialTime">initialTime от 0 до 1</param>
        /// <returns></returns>
        private async UniTask ForceJump(float initialTime = 0.0f)
        {
            if (_jumping)
            {
                _jumping = false;
                await StopJumps();
            }

            _jumping = true;
            _rb.gravityScale = 0f;

            Func<float, float> avFunc = ctx => jumpHeight * jumpCurve.Evaluate(ctx / jumpTime);
            var initialMaxedTime = Mathf.Clamp01(initialTime) * jumpTime;
            var t = initialMaxedTime;
            var prev = t;
            CurrentJumpTime = t;
            var initialYPos = _rb.position.y - avFunc(initialMaxedTime);

            for (; t < jumpTime; t += Time.fixedDeltaTime)
            {
                CurrentJumpTime = t;
                if (_stopAllJumps) return;
                while (!Available())
                {
                    if (_stopAllJumps) return;
                    initialYPos = _rb.position.y - avFunc(initialMaxedTime) - avFunc(t);
                    await UniTask.WaitForFixedUpdate();
                }

                prev = (t - Time.fixedDeltaTime) / jumpTime;
                if (t > jumpTime / 5f && _collideCheck.IsTouchingGround)
                {
                    prev = (t - Time.fixedDeltaTime) / jumpTime;
                    break;
                }

                if (_collideCheck.IsTouchingTop && t < whenMax * jumpTime)
                {
                    var timesEquals = new List<float>(0);
                    for (var tt = 0f; tt < jumpTime; tt += Time.fixedDeltaTime)
                    {
                        if (Math.Abs(avFunc(tt) - avFunc(t)) < Time.fixedDeltaTime) timesEquals.Add(tt);
                    }

                    t = timesEquals.Last();
                    CurrentJumpTime = t;
                }

                _rb.position = new Vector2(_rb.position.x, initialYPos + avFunc(t));
                await UniTask.WaitForFixedUpdate();
            }

            CurrentJumpTime = jumpTime;

            if (_stopAllJumps) return;
            _rb.velocity += new Vector2(0, (avFunc(t) - avFunc(prev * jumpTime)) / (t - prev * jumpTime));
            _rb.gravityScale = 1f;
            _jumping = false;
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

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Vector2 Multiply(this Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
        public static Vector3 Multiply(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vector2 Multiply(this Vector2 a, float bx, float by) => new Vector2(a.x * bx, a.y * by);
        public static Vector3 Multiply(this Vector3 a, float bx, float by, float bz) => new Vector3(a.x * bx, a.y * by, a.z * bz);
        public static Vector2 Divide(this Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);
        public static Vector3 Divide(this Vector3 a, Vector3 b) => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);


        public static void DrawBox(Vector2 point, Vector2 size)
        {
#if UNITY_EDITOR
            Debug.DrawLine(point + new Vector2(size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, size.y / 2f));
#endif
        }
    }
}