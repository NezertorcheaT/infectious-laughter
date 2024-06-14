using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement Ability")]
    public class EntityMovementHorizontalMove : Ability
    {
        [SerializeField] private float speed;
        public bool RightTurn = true; // true - последнее направление это право, false соответственно лево
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float velocity)
        {
            if (!Available()) return;
            // опять база от липтона?
            if(velocity > 0)
            {
                RightTurn = true;
            }else if(velocity < 0)
            {
                RightTurn = false;
            }
            _rb.velocity = new Vector2(velocity * speed, _rb.velocity.y);
        }
    }
}