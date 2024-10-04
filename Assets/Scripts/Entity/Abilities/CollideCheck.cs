using System.Collections.Generic;
using CustomHelper;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Collide Check")]
    public class CollideCheck : Ability
    {
        [SerializeField] private int maxSlopeAngle;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField, Min(0.01f)] private float groundDistance = 0.1f;
        [SerializeField] Collider2D collider;
        private float _colliderOffset = 0;

        public bool IsTouchingGround
        {
            get
            {
                collider ??= GetComponent<Collider2D>();
                var checkSize = new Vector3(collider.bounds.size.x - 2f * _colliderOffset, groundDistance);
                var size = transform.lossyScale;
                var checkPosition =
                    transform.position +
                    new Vector3(0, -collider.bounds.size.y / 2f) +
                    (Vector3)collider.offset.Multiply(size) -
                    new Vector3(0, groundDistance / 2f / size.y + _colliderOffset);

                return Overlap(checkPosition, checkSize);
            }
        }

        public bool IsTouchingTop
        {
            get
            {
                collider ??= GetComponent<Collider2D>();
                var checkSize = new Vector3(collider.bounds.size.x - 2f * _colliderOffset, groundDistance);
                var size = transform.lossyScale;
                var checkPosition =
                    transform.position +
                    new Vector3(0, collider.bounds.size.y / 2f) +
                    (Vector3)collider.offset.Multiply(size) +
                    new Vector3(0, groundDistance / 2f / size.y + _colliderOffset);

                return Overlap(checkPosition, checkSize);
            }
        }

        public bool IsTouchingRight
        {
            get
            {
                collider ??= GetComponent<Collider2D>();
                var checkSize = new Vector3(groundDistance, collider.bounds.size.y - 2f * _colliderOffset);
                var size = transform.lossyScale;
                var checkPosition =
                    transform.position +
                    new Vector3(collider.bounds.size.x / 2f, 0) +
                    (Vector3)collider.offset.Multiply(size) +
                    new Vector3(groundDistance / 2f / size.x + _colliderOffset, 0);

                return Overlap(checkPosition, checkSize);
            }
        }

        public bool IsTouchingLeft
        {
            get
            {
                collider ??= GetComponent<Collider2D>();
                var checkSize = new Vector3(groundDistance, collider.bounds.size.y - 2f * _colliderOffset);
                var size = transform.lossyScale;
                var checkPosition =
                    transform.position -
                    new Vector3(collider.bounds.size.x / 2f, 0) +
                    (Vector3)collider.offset.Multiply(size) -
                    new Vector3(groundDistance / 2f / size.x + _colliderOffset, 0);

                return Overlap(checkPosition, checkSize);
            }
        }

        public bool IsOnWall => IsTouchingLeft || IsTouchingRight;

        private bool Overlap(Vector2 position, Vector2 size)
        {
            Helper.DrawBox(position, size);
            return Physics2D.OverlapBoxAll(
                    position,
                    size,
                    0,
                    groundLayer.value)
                .Length > 0;
        }

        public bool IsTouching(IEnumerable<ContactPoint2D> points, Vector2 direction)
        {
            foreach (var contact in points)
            {
                if (Vector2.Angle(direction.normalized, contact.normal) >= maxSlopeAngle) continue;
                return true;
            }

            return false;
        }
    }
}