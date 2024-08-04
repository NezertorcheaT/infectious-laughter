using System;
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
        private TreeOfWisdomUseAbility _useTreeOfWisdom;
        private EntityMovementHorizontalMove _moveAbility;
        private IJumpableAbility _jumpAbility;
        private EntityMovementCrouch _crouchAbility;
        private EntityGarbage _entityGarbage;
        private EntityMovementDowning _movementDowning;
        private CollideCheck _collideCheck;
        private DashAbility _dashAbility;
        private PlayerCameraFollowPointAbility _playerFollowPoint;

        public override void Initialize()
        {
            base.Initialize();
            _useTreeOfWisdom = Entity.FindAbilityByType<TreeOfWisdomUseAbility>();
            _entityGarbage = Entity.FindExactAbilityByType<EntityGarbage>();
            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _crouchAbility = Entity.FindAbilityByType<EntityMovementCrouch>();
            _movementDowning = Entity.FindExactAbilityByType<EntityMovementDowning>();
            _collideCheck = Entity.FindExactAbilityByType<CollideCheck>();
            _dashAbility = Entity.FindExactAbilityByType<DashAbility>();
            _playerFollowPoint = Entity.FindAbilityByType<PlayerCameraFollowPointAbility>();
            
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
            _actions.Gameplay.PickGarbage.performed += UseTreeOfWisdomPerformed;
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

        private void UseTreeOfWisdomPerformed(InputAction.CallbackContext ctx) => _useTreeOfWisdom.UseTree();
        private void CrouchOnCanceled(InputAction.CallbackContext ctx) => _crouchAbility.UnCrouch();
        private void CrouchOnStarted(InputAction.CallbackContext ctx) => _crouchAbility.Crouch();
        private void PickGarbagePerformed(InputAction.CallbackContext ctx) => _entityGarbage.PickGarbage();
        private void JumpOnPerformed(InputAction.CallbackContext ctx) => _jumpAbility.Jump();
        private void DashOnPerformed(InputAction.CallbackContext ctx) => _dashAbility.Dash();


        private bool _hangingRight;
        private bool _hangingLeft;
        private float _input;

        private void Move()
        {
            _input = _actions.Gameplay.Move.ReadValue<float>();
            _moveAbility.Move(_input);
            _movementDowning.WallDowning(_input);
            _playerFollowPoint.MovePoint(_input);
        }

    }
}