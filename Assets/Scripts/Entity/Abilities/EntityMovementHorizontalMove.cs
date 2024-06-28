using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement Ability")]
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField] public float Speed;
        private Rigidbody2D _rb;

        public bool RightTurn { get; private set; }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float velocity)
        {
            if (!Available() || Mathf.Abs(_rb.velocity.x) > Speed) return;
            RightTurn = velocity == 0 ? RightTurn : velocity > 0;
            _rb.velocity = new Vector2(velocity * Speed, _rb.velocity.y);
        }
    }
}