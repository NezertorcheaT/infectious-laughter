using Entity.EntityMovement;

namespace Entity.EntityControllers
{
    public class ControllerInput : Controller
    {
        private Controls _actions;
        
        // Cache
        private EntityMovementHorizontalMove _moveAbility;
        private EntityMovementJump _jumpAbility;
        private EntityMovementCrouch _crouchAbility;

        public override void Initialize()
        {
            base.Initialize();
            _actions = new Controls();

            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _jumpAbility = Entity.FindAbilityByType<EntityMovementJump>();
            _crouchAbility = Entity.FindAbilityByType<EntityMovementCrouch>();

            OnEnable();
        }

        private void OnEnable()
        {
            if (_actions == null) return;
            _actions.Enable();

            Entity.OnFixedUpdate += Move;
            _actions.Gameplay.Jump.performed += ctx => _jumpAbility.Jump();
            _actions.Gameplay.Crouch.started += ctx => _crouchAbility.Crouch();
            _actions.Gameplay.Crouch.canceled += ctx => _crouchAbility.UnCrouch();
        }
        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.Disable();

            Entity.OnFixedUpdate -= Move;
            _actions.Gameplay.Jump.performed -= ctx => _jumpAbility.Jump();
        }

        private void Move() => _moveAbility.Move(_actions.Gameplay.Move.ReadValue<float>());
    }
}