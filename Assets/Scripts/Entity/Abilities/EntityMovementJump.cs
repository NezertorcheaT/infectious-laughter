using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Jump Ability")]
    public class EntityMovementJump : Ability
    {

        [Space(10.0f)] [SerializeField] private float jumpHeight = 5f;

        [SerializeField, Min(0.69f), Tooltip("Значения меньше 0.69 могут обосрать платформы")]
        
        private float jumpTime = 1f;

        [
            SerializeField,
            Tooltip("Выставьте значение от 0 до 1, при котором на графике находится максимальная точка"),
            Range(0f, 1f)
        ]
        
        private float whenMax = 0.5f;

        //private float groundDistance = 0.1f;

        [SerializeField] private LayerMask groundLayer;
        [Space(10.0f), SerializeField, Min(1)] private int jumpsCount = 1;

        private int curJumpsCount;

        private Rigidbody2D _rb;
        private Collider2D _col;

        public float JumpTime => jumpTime;
        //public float CurrentJumpTime { get; private set; }
        public float JumpHeight => jumpHeight;
        public float WhenMax => whenMax;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();

            curJumpsCount = jumpsCount;
        }

        public async UniTask Jump(bool force = false)
        {
            if (!Available()) return;
            if (curJumpsCount == 0 && !force)
            {
                if (!CheckGround(Entity.CachedTransform.position, Entity.CachedTransform.lossyScale, _col, groundLayer,
                    0.1f)) return;
                curJumpsCount = jumpsCount;
            }
            
            curJumpsCount--;
            await ForceJump();
        }

        private bool _jumping = false;
        private bool _stopAllJumps = false;

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
            }

            _jumping = true;
            _rb.gravityScale = 1f;


            var initialYPos = _rb.position.y;
            
            

                //_rb.position = new Vector2(_rb.position.x, initialYPos);
            
            
            //_rb.velocity += new Vector2(0, initialYPos * jumpHeight * 100 * Time.deltaTime);
            //_rb.position = new Vector2(_rb.position.x, initialYPos);

            if (_stopAllJumps) return;
            // Блябуду где то че то сломалось потому что я не поверю что это делается 1 строчкой :(
            _rb.velocity += new Vector2(0, initialYPos * jumpHeight * 100 * Time.deltaTime);
            Debug.Log("Проигралась функция прыжка типа ");
            //_rb.AddForce(new Vector2(0, initialYPos * jumpHeight * 10 * Time.deltaTime), ForceMode2D.Impulse);
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