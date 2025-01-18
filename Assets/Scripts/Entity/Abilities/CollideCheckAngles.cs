using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Collide Check By Angles")]
    public class CollideCheckAngles : CollideCheck
    {
        [SerializeField] protected int maxWallAngle = 90;
        [SerializeField] protected int maxSlopeAngle = 80;
        [SerializeField] protected LayerMask groundLayer;

        public override bool IsTouchingGround => IsTouching(WallType.Ground);
        public override bool IsTouchingTop => IsTouching(WallType.Top);
        public override bool IsTouchingRight => IsTouching(WallType.Right);
        public override bool IsTouchingLeft => IsTouching(WallType.Left);

        private Direction[] _directions;

        private enum WallType
        {
            Ground,
            Top,
            Right,
            Left
        }

        private void Awake()
        {
            _directions = new Direction[Enum.GetNames(typeof(WallType)).Length];

            for (int i = 0; i < _directions.Length; i++)
            {
                _directions[i] = new();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer != groundLayer)
                return;

            HandleCollision(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer != groundLayer)
                return;

            RemoveCollision(collision);
        }


        private void HandleCollision(Collision2D collision)
        {
            HashSet<Direction> alreadyAdded = new(_directions.Length);
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Direction contactDirection = GetDirection(GetWallType(contact));
                contactDirection.AddCollision(collision);
                alreadyAdded.Add(contactDirection);

                RemoveCollision(collision, alreadyAdded);
            }
        }

        private void RemoveCollision(Collision2D collision, IEnumerable<Direction> except = null)
        {
            foreach (Direction dir in _directions)
            {
                if (except != null && except.Contains(dir))
                    continue;

                dir.RemoveCollisionIfContains(collision);
            }
        }

        private WallType GetWallType(ContactPoint2D contact)
        {
            Vector2 normal = contact.normal;

            if (Vector2.Angle(Vector2.up, normal) <= maxSlopeAngle)
                return WallType.Ground;

            else if (Vector2.Angle(Vector2.up, normal) <= maxWallAngle)
            {
                if (contact.point.x > transform.position.x)
                    return WallType.Right;
                else
                    return WallType.Left;
            }

            else
                return WallType.Top;
        }

        private bool IsTouching(WallType type)
            => GetDirection(type).IsTouching;

        private Direction GetDirection(WallType type)
            => _directions[(int)type];

        private class Direction
        {
            public bool IsTouching;
            private readonly HashSet<Collider2D> _colliders = new(4);

            public void AddCollision(Collision2D collision)
            {
                _colliders.Add(collision.collider);
                IsTouching = true;
            }

            public void RemoveCollisionIfContains(Collision2D collision)
            {
                _colliders.Remove(collision.collider);

                if (_colliders.Count == 0)
                    IsTouching = false;
            }

            public IReadOnlyCollection<Collider2D> GetColliders() => _colliders;
        }
    }
}