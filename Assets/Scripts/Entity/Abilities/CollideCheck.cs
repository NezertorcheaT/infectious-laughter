using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Collide Check Ability")]
    public class CollideCheck : Ability
    {
        [SerializeField] private int maxSlopeAngle;

        public bool IsTouchingGround { get; private set; }
        public bool IsTouchingTop { get; private set; }
        public bool IsTouchingRight { get; private set; }
        public bool IsTouchingLeft { get; private set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IsTouchingGround = IsTouchingGround != IsTouching(collision.contacts, Vector2.up);
            IsTouchingTop = IsTouchingTop != IsTouching(collision.contacts, Vector2.down);
            IsTouchingRight = IsTouchingRight != IsTouching(collision.contacts, Vector2.left);
            IsTouchingLeft = IsTouchingLeft != IsTouching(collision.contacts, Vector2.right);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IsTouchingGround = IsTouching(collision.contacts, Vector2.up);
            IsTouchingTop = IsTouching(collision.contacts, Vector2.down);
            IsTouchingRight = IsTouching(collision.contacts, Vector2.left);
            IsTouchingLeft = IsTouching(collision.contacts, Vector2.right);
        }

        private bool IsTouching(IEnumerable<ContactPoint2D> points, Vector2 direction)
        {
            foreach (var contact in points)
            {
                if (Vector2.Angle(direction.normalized, contact.normal) >= maxSlopeAngle) continue;
                return true;
            }

            return false;
        }

        public int RightTrajectory(float inputVelocity)
        {
            int answ = 0;
            if (IsTouchingLeft && inputVelocity < 0) answ = -1;
            else if (IsTouchingRight && inputVelocity > 0) answ = 1;
            return answ;
        }

        public bool TestOnWall()
        {
            return (IsTouchingLeft || IsTouchingRight);
        }
    }
}