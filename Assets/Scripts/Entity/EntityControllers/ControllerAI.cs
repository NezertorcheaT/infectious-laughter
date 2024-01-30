using Entity.EntityMovement;
using UnityEngine;

namespace Entity.EntityControllers
{
    [RequireComponent(typeof(Collider2D))]
    public class ControllerAI : Controller
    {
        private Collider2D _coll;
        private EntityMovementHorizontalMove _moveAbility;
        private EntityMovementJump _jumpAbility;
        private bool _direction;
        [SerializeField] private float rayDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        public override void Initialize()
        {
            base.Initialize();

            _coll = GetComponent<Collider2D>();

            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _jumpAbility = Entity.FindAbilityByType<EntityMovementJump>();
            OnInitializationComplete += OnEnable;
            OnEnable();
        }

        private void OnEnable()
        {
            if (!IsInitialized) return;
            Entity.OnFixedUpdate += Move;
            OnInitializationComplete -= OnEnable;
        }

        private void OnDisable() => Entity.OnFixedUpdate -= Move;

        private void Move()
        {
            var ray = new Ray(
                transform.position + (Vector3) _coll.offset +
                _coll.bounds.size.Multiply(new Vector3(_direction ? 1 : -1, -1f / 2f, 1)),
                Vector3.down);
            if (!Physics2D.Raycast(ray.origin, ray.direction, rayDistance, groundLayer))
                _direction = !_direction;
            Debug.DrawRay(ray.origin, ray.direction * rayDistance);

            _moveAbility.Move(_direction ? 1 : -1);
        }
    }
}