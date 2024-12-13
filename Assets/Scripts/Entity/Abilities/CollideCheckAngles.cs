using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Collide Check By Angles")]
    public class CollideCheckAngles : CollideCheck
    {
        [SerializeField] protected int maxWallAngle = 90;

        public override bool IsTouchingGround => IsTouching(WallType.Ground);
        public override bool IsTouchingTop => IsTouching(WallType.Top);
        public override bool IsTouchingRight => IsTouching(WallType.Right);
        public override bool IsTouchingLeft => IsTouching(WallType.Left);

        private Direction[] directions;

        public enum WallType
        {
            Ground,
            Top,
            Right,
            Left
        }


        private void Awake()
        {
            directions = new Direction[Enum.GetNames(typeof(WallType)).Length];

            for (int i = 0; i < directions.Length; i++)
            {
                directions[i] = new();
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
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Direction contactDirection = GetDirection(GetWallType(contact));
                contactDirection.AddCollision(collision);

                RemoveCollision(collision, contactDirection);
            }
        }

        private void RemoveCollision(Collision2D collision, params Direction[] except)
        {
            foreach (Direction dir in directions)
            {
                if (except.Contains(dir))
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
                if (contact.point.x > Entity.CachedTransform.position.x) 
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
                            => directions[(int)type];



        private class Direction
        {
            protected readonly HashSet<Collider2D> colliders;

            public bool IsTouching;


            public void AddCollision(Collision2D collision)
            {
                // Мы не будем вызывать Contains потому что Add уже проверяет на это
                colliders.Add(collision.collider);

                IsTouching = true;

                return;
            }

            public void RemoveCollisionIfContains(Collision2D collision)
            {
                // Мы не будем вызывать Contains потому что Remove уже проверяет на это
                colliders.Remove(collision.collider);

                if (colliders.Count == 0)
                    IsTouching = false;
            }

            public IReadOnlyCollection<Collider2D> GetColliders() => colliders;
        }
    }
}