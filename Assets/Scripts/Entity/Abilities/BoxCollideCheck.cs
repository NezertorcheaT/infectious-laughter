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
        [SerializeField] private LayerMask groundLayer;
        [SerializeField, Min(0.01f)] private float groundDistance = 0.1f;
        [SerializeField] private new Collider2D collider;
        [SerializeField] private float colliderOffset;

        private Vector2 _checkSize;
        private Vector3 _size;
        private Vector2 _initialPosition;
        private Vector3 _boundsSize;
        private Collider2D[] _colliders;

        private void Start()
        {
            _colliders = new Collider2D[10];
        }

        private void FixedUpdate()
        {
            if (!Available()) return;

            collider ??= GetComponent<Collider2D>();
            _size = Entity.CachedTransform.lossyScale;
            _initialPosition = Entity.CachedTransform.position + (Vector3)collider.offset.Multiply(_size);
            _boundsSize = collider.bounds.size;

            _checkSize = new Vector2(_boundsSize.x - 2f * colliderOffset, groundDistance);
            _isTouchingGround = Overlap(
                _initialPosition
                - new Vector2(0, _boundsSize.y / 2f)
                - new Vector2(0, groundDistance / 2f / _size.y + colliderOffset)
            );
            _isTouchingTop = Overlap(
                _initialPosition
                + new Vector2(0, _boundsSize.y / 2f)
                + new Vector2(0, groundDistance / 2f / _size.y + colliderOffset)
            );

            _checkSize = new Vector2(groundDistance, _boundsSize.y - 2f * colliderOffset);
            _isTouchingRight = Overlap(
                _initialPosition
                + new Vector2(_boundsSize.x / 2f, 0)
                + new Vector2(groundDistance / 2f / _size.x + colliderOffset, 0)
            );
            _isTouchingLeft = Overlap(
                _initialPosition
                - new Vector2(_boundsSize.x / 2f, 0)
                - new Vector2(groundDistance / 2f / _size.x + colliderOffset, 0)
            );
        }

        public override bool IsTouchingGround => _isTouchingGround;
        private bool _isTouchingGround;

        public override bool IsTouchingTop => _isTouchingTop;
        private bool _isTouchingTop;

        public override bool IsTouchingRight => _isTouchingRight;
        private bool _isTouchingRight;

        public override bool IsTouchingLeft => _isTouchingLeft;
        private bool _isTouchingLeft;

        private bool Overlap(Vector2 position)
        {
            Helper.DrawBox(position, _checkSize);
            var count = Physics2D.OverlapBoxNonAlloc(
                position,
                _checkSize,
                0,
                _colliders,
                groundLayer.value
            );
            return _colliders.Take(count).Any(i => !i.isTrigger && !i.usedByEffector);
        }
    }
}