using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement Ability")]
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField, Min(0.001f)] private float speed;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        public bool Turn { get; private set; }

        public float TurnInFloat { get; private set; }

        public float Speed
        {
            get => speed;
            set
            {
                enabled = value > 0;
                speed = Mathf.Max(value, 0);
            }
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Move(float velocity)
        {
            if (!Available()) return;
            TurnInFloat = velocity;
            Turn = velocity == 0 ? Turn : velocity > 0;
            _rb.velocity = new Vector2(velocity * speed, _rb.velocity.y);
            _spriteRenderer.flipX = !Turn;
        }
    }
}