using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement Ability")]
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rb;

        public bool RightTurn { get; private set; }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float velocity)
        {
            if (!Available() || Mathf.Abs(_rb.velocity.x) > speed) return;
            RightTurn = velocity == 0 ? RightTurn : velocity > 0;
            _rb.velocity = new Vector2(velocity * speed, _rb.velocity.y);
        }
    }
}