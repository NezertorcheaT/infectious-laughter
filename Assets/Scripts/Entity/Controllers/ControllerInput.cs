using Entity.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entity.Controllers
{
    [AddComponentMenu("Entity/Controllers/Input Controller")]
    public class ControllerInput : Controller
    {
        [Inject] private Controls _actions;

        // Cache
        private EntityMovementHorizontalMove _moveAbility;
        private EntityMovementJump _jumpAbility;
        private EntityMovementCrouch _crouchAbility;

        public override void Initialize()
        {
            base.Initialize();
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
            _actions.Gameplay.Jump.performed += JumpOnPerformed;
            _actions.Gameplay.Crouch.started += CrouchOnStarted;
            _actions.Gameplay.Crouch.canceled += CrouchOnCanceled;
        }

        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.Disable();

            Entity.OnFixedUpdate -= Move;
            _actions.Gameplay.Jump.performed -= JumpOnPerformed;
            _actions.Gameplay.Crouch.started -= CrouchOnStarted;
            _actions.Gameplay.Crouch.canceled -= CrouchOnCanceled;
        }

        private void CrouchOnCanceled(InputAction.CallbackContext ctx) => _crouchAbility.UnCrouch();
        private void CrouchOnStarted(InputAction.CallbackContext ctx) => _crouchAbility.Crouch();
        private void JumpOnPerformed(InputAction.CallbackContext ctx) => _jumpAbility.Jump();

        private void Move() => _moveAbility.Move(_actions.Gameplay.Move.ReadValue<float>());
    }
}