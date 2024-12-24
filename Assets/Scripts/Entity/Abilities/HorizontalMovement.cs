using NaughtyAttributes;
using System;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Horizontal Movement")]
    public class HorizontalMovement : Ability
    {
        [SerializeField, Min(0.001f)] private float speed;
        [SerializeField] private bool canUseDash;
        [SerializeField] private bool turn;

        [SerializeField, EnableIf("canUseDash"), Min(0.001f)]
        private float dashSpeed;

        [SerializeField, EnableIf("canUseDash")]
        private float maxDashDistance;

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

        public void Move(float velocity, float distance)
        {
            if (!Available() || Mathf.Abs(_rb.velocity.x) > speed) return;

            TurnInFloat = velocity;
            Turn = velocity == 0 ? Turn : velocity > 0;

            _rb.velocity = distance < maxDashDistance && canUseDash
                ? new Vector2(velocity * dashSpeed, _rb.velocity.y)
                : new Vector2(velocity * speed, _rb.velocity.y);
        }
    }
}