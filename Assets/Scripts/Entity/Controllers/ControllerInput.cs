using System.Threading.Tasks;
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
        [SerializeField, Min(0)] private float sizeX = .1f;
        [SerializeField, Min(0)] private float sizeY = .1f;
        [SerializeField, Min(.1f)] private float downingTime = 3f;
        [SerializeField] private LayerMask groundLayer;

        // Cache
        private EntityMovementHorizontalMove _moveAbility;
        private PlayerJumpAbility _jumpAbility;
        private EntityMovementCrouch _crouchAbility;
        private EntityMovementDowning _downingAbility;
        private Collider2D _collider;
        private Rigidbody2D _rb;

        public override void Initialize()
        {
            base.Initialize();
            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _jumpAbility = Entity.FindAbilityByType<PlayerJumpAbility>();
            _crouchAbility = Entity.FindAbilityByType<EntityMovementCrouch>();
            _downingAbility = Entity.FindAbilityByType<EntityMovementDowning>();
            _collider = Entity.GetComponent<Collider2D>();
            _rb = Entity.GetComponent<Rigidbody2D>();

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

        private bool _hangingRight;
        private bool _hangingLeft;
        private float _input;

        private void Move()
        {
            _input = _actions.Gameplay.Move.ReadValue<float>();
            _moveAbility.Move(_input);
        }

    }
}