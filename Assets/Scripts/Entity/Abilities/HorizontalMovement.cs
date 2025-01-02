using System;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement")]
    public class HorizontalMovement : Ability
    {
        [SerializeField, Min(0.001f)] private float speed;
        [SerializeField] private bool turn;
        private Rigidbody2D _rb;

        public bool Turn
        {
            get => turn;
            private set
            {
                turn = value;
                if (TurnInFloat != 0f)
                    OnTurn?.Invoke(turn);
            }
        }

        public float TurnInFloat { get; private set; }
        public event Action<bool> OnTurn;

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
        }

        public void Move(float velocity)
        {
            var push = velocity * speed;

            TurnInFloat = velocity;
            Turn = velocity == 0 ? Turn : velocity > 0;

            if (!Available() || (
                    (_rb.velocity.x >= 0 || push <= 0) &&
                    (_rb.velocity.x <= 0 || push >= 0) &&
                    push != 0 &&
                    Mathf.Abs(_rb.velocity.x) > Mathf.Abs(push)
                )) return;

            _rb.velocity = new Vector2(push, _rb.velocity.y);
        }
    }
}