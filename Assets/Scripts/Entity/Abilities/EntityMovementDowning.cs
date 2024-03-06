using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Downing Ability")]
    public class EntityMovementDowning : Ability
    {
        [SerializeField] private float speed;

        private Rigidbody2D _rb;

        public override void Initialize()
        {
            base.Initialize();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!Available()) return;

            _rb.velocity = new Vector2(0, -speed);
        }
    }
}