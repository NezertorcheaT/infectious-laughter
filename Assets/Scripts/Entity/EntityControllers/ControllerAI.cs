using Entity.EntityMovement;
using UnityEngine;

namespace Entity.EntityControllers
{
    public class ControllerAI : Controller
    {
        [SerializeField] private Collider2D coll;
        private EntityMovementHorizontalMove _moveAbility;
        private EntityMovementJump _jumpAbility;

        public override void Initialize()
        {
            base.Initialize();

            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _jumpAbility = Entity.FindAbilityByType<EntityMovementJump>();

            OnEnable();
        }

        private void OnEnable()
        {
            if (IsInitialized) Entity.OnFixedUpdate += Move;
        }

        private void OnDisable() => Entity.OnFixedUpdate -= Move;

        private void Move()
        {
            //_moveAbility.Move(1);
            Debug.DrawRay(
                transform.position + 
                (Vector3) coll.offset + coll.bounds.size / 2f
                
                , Vector3.down);
        }
    }
}