using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement Ability")]
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField] private float speed;
        [SerializeField] public bool CanWalk = true;
        private bool entityHasStop = false;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float velocity)
        {
            if (!Available()) return;
            if (CanWalk)
            {
                _rb.velocity = new Vector2(velocity * speed, _rb.velocity.y);
                entityHasStop = false;
            }else if(!entityHasStop)
            {
                _rb.velocity += -_rb.velocity;
                entityHasStop = true;
            }
        }
    }
}