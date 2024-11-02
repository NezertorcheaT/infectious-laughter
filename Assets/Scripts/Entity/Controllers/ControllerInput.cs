using Entity.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entity.Controllers
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Controllers/Input Controller")]
    public class ControllerInput : Controller
    {
        [Inject] private Controls _actions;

        // Cache
        private InteractivePropsUsing _useInteractiveProps;
        private HorizontalMovement _moveAbility;
        private IJumpableAbility _jumpAbility;
        private Crouching _crouchAbility;
        private Garbage _entityGarbage;
        private Downing _movementDowning;
        private CollideCheck _collideCheck;
        private Dash _dashAbility;
        private CameraFollowPoint _followPoint;

        public override void Initialize()
        {
            base.Initialize();
            _useInteractiveProps = Entity.FindExactAbilityByType<InteractivePropsUsing>();
            _entityGarbage = Entity.FindExactAbilityByType<Garbage>();
            _moveAbility = Entity.FindAbilityByType<HorizontalMovement>();
            _crouchAbility = Entity.FindAbilityByType<Crouching>();
            _movementDowning = Entity.FindExactAbilityByType<Downing>();
            _collideCheck = Entity.FindExactAbilityByType<CollideCheck>();
            _dashAbility = Entity.FindExactAbilityByType<Dash>();
            _followPoint = Entity.FindAbilityByType<CameraFollowPoint>();
            
            OnEnable();
        }

        private void Start()
        {
            _jumpAbility = Entity.FindAvailableAbilityByInterface<IJumpableAbility>();
        }

        private void OnEnable()
        {
            if (_actions == null) return;
            _actions.Enable();

            Entity.OnFixedUpdate += Move;
            _actions.Gameplay.Dash.performed += DashOnPerformed;
            _actions.Gameplay.Jump.performed += JumpOnPerformed;
            _actions.Gameplay.PickGarbage.performed += UseInteractivePropsPerformed;
            _actions.Gameplay.PickGarbage.performed += PickGarbagePerformed;
            _actions.Gameplay.Crouch.started += CrouchOnStarted;
            _actions.Gameplay.Crouch.canceled += CrouchOnCanceled;
        }

        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.Disable();

            Entity.OnFixedUpdate -= Move;
            _actions.Gameplay.Dash.performed -= DashOnPerformed;
            _actions.Gameplay.Jump.performed -= JumpOnPerformed;
            _actions.Gameplay.Crouch.started -= CrouchOnStarted;
            _actions.Gameplay.Crouch.canceled -= CrouchOnCanceled;
        }

        private void UseInteractivePropsPerformed(InputAction.CallbackContext ctx) => _useInteractiveProps.UseInteractiveProps();
        private void CrouchOnCanceled(InputAction.CallbackContext ctx) => _crouchAbility.UndoPerform();
        private void CrouchOnStarted(InputAction.CallbackContext ctx) => _crouchAbility.Perform();
        private void PickGarbagePerformed(InputAction.CallbackContext ctx) => _entityGarbage.PickGarbage();
        private void JumpOnPerformed(InputAction.CallbackContext ctx) => _jumpAbility.Perform();
        private void DashOnPerformed(InputAction.CallbackContext ctx) => _dashAbility.Perform();


        private bool _hangingRight;
        private bool _hangingLeft;
        private float _input;
        private float _inputY;

        private void Move()
        {
            _input = _actions.Gameplay.Move.ReadValue<float>();
            _inputY = _actions.Gameplay.VerticalMove.ReadValue<float>();
            _moveAbility.Move(_input, 0);
            _movementDowning.WallDowning(_input);
            _followPoint.MovePoint(_input, _inputY);
        }

    }
}