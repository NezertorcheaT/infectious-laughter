using System;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Entity.Abilities
{
    public abstract class CollideCheck : Ability
    {
        public abstract bool IsTouchingGround { get; }
        public abstract bool IsTouchingTop { get; }
        public abstract bool IsTouchingRight { get; }
        public abstract bool IsTouchingLeft { get; }
        public bool IsOnWall => IsTouchingLeft || IsTouchingRight;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Box Collide Check")]
    public class BoxCollideCheck : CollideCheck
    {
        [SerializeField] private int maxSlopeAngle;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField, Min(0.01f)] private float groundDistance = 0.1f;
        [SerializeField] private Collider2D collider;
        [SerializeField] private float colliderOffset;

        private Vector3 _checkSize;
        private Vector3 _checkPosition;
        private Vector3 _size;
        private Collider2D[] _colliders;

        private void Start()
        {
            _colliders = new Collider2D[10];
        }

        private void FixedUpdate()
        {
            collider ??= GetComponent<Collider2D>();
            _size = Entity.CachedTransform.lossyScale;

            _checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            _checkPosition =
                transform.position +
                new Vector3(0, -collider.bounds.size.y / 2f) +
                (Vector3)collider.offset.Multiply(_size) -
                new Vector3(0, groundDistance / 2f / _size.y + colliderOffset);
            _isTouchingGround = Overlap(_checkPosition, _checkSize);

            _checkSize = new Vector3(collider.bounds.size.x - 2f * colliderOffset, groundDistance);
            _checkPosition =
                transform.position +
                new Vector3(0, collider.bounds.size.y / 2f) +
                (Vector3)collider.offset.Multiply(_size) +
                new Vector3(0, groundDistance / 2f / _size.y + colliderOffset);
            _isTouchingTop = Overlap(_checkPosition, _checkSize);

            _checkSize = new Vector3(groundDistance, collider.bounds.size.y - 2f * colliderOffset);
            _checkPosition =
                transform.position +
                new Vector3(collider.bounds.size.x / 2f, 0) +
                (Vector3)collider.offset.Multiply(_size) +
                new Vector3(groundDistance / 2f / _size.x + colliderOffset, 0);
            _isTouchingRight = Overlap(_checkPosition, _checkSize);

            _checkSize = new Vector3(groundDistance, collider.bounds.size.y - 2f * colliderOffset);
            _checkPosition =
                transform.position -
                new Vector3(collider.bounds.size.x / 2f, 0) +
                (Vector3)collider.offset.Multiply(_size) -
                new Vector3(groundDistance / 2f / _size.x + colliderOffset, 0);
            _isTouchingLeft = Overlap(_checkPosition, _checkSize);
        }

        public override bool IsTouchingGround => _isTouchingGround;
        private bool _isTouchingGround;

        public override bool IsTouchingTop => _isTouchingTop;
        private bool _isTouchingTop;

        public override bool IsTouchingRight => _isTouchingRight;
        private bool _isTouchingRight;

        public override bool IsTouchingLeft => _isTouchingLeft;
        private bool _isTouchingLeft;

        private bool Overlap(Vector2 position, Vector2 size)
        {
            Helper.DrawBox(position, size);
            var count = Physics2D.OverlapBoxNonAlloc(
                position,
                size,
                0,
                _colliders,
                groundLayer.value
            );
            return _colliders.Take(count).Any(i => !i.isTrigger && !i.usedByEffector);
        }
    }
}