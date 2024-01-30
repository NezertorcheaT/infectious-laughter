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
            //_moveAbility.Move(1);
            var ray = new Ray(
                transform.position + (Vector3) _coll.offset + _coll.bounds.size / 2f,
                Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction);
        }
    }
}