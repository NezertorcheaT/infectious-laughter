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
        private EntityMovementJump _jumpAbility;
        private EntityMovementCrouch _crouchAbility;
        private EntityMovementDowning _downingAbility;
        private Collider2D _collider;
        private Rigidbody2D _rb;

        public override void Initialize()
        {
            base.Initialize();
            _moveAbility = Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            _jumpAbility = Entity.FindAbilityByType<EntityMovementJump>();
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
            var bounds = _collider.bounds;
            var size = new Vector2(sizeX, sizeY);

            var rightDown = bounds.center + new Vector3(bounds.size.x / 2f, -bounds.size.y / 2f) +
                            new Vector3(sizeX / 2f, sizeY / 2f);
            var leftDown = bounds.center + new Vector3(-bounds.size.x / 2f, -bounds.size.y / 2f) +
                           new Vector3(-sizeX / 2f, sizeY / 2f);
            var rightUp = bounds.center + new Vector3(bounds.size.x / 2f, bounds.size.y / 2f) +
                          new Vector3(sizeX / 2f, -sizeY / 2f);
            var leftUp = bounds.center + new Vector3(-bounds.size.x / 2f, bounds.size.y / 2f) +
                         new Vector3(-sizeX / 2f, -sizeY / 2f);

#if UNITY_EDITOR
            Helper.DrawBox(rightDown, size);
            Helper.DrawBox(leftDown, size);
            Helper.DrawBox(rightUp, size);
            Helper.DrawBox(leftUp, size);    
#endif

            var rightDownCheck = Physics2D.OverlapBoxAll(rightDown, size, 0, groundLayer).Length > 0;
            var leftDownCheck = Physics2D.OverlapBoxAll(leftDown, size, 0, groundLayer).Length > 0;
            var rightUpCheck = Physics2D.OverlapBoxAll(rightUp, size, 0, groundLayer).Length > 0;
            var leftUpCheck = Physics2D.OverlapBoxAll(leftUp, size, 0, groundLayer).Length > 0;

            _moveAbility.Move(_input);

            if (rightDownCheck && leftDownCheck)
            {
                _hangingLeft = false;
                _hangingRight = false;
                return;
            }

            if (_hangingLeft)
            {
                if (_input > 0)
                {
                    _hangingLeft = false;
                    return;
                }
            }
            else if (leftDownCheck && leftUpCheck && !(_hangingRight && _hangingLeft))
            {
                _hangingLeft = true;
                Downing(false);
                return;
            }

            if (_hangingRight)
            {
                if (_input < 0)
                {
                    _hangingRight = false;
                    return;
                }
            }
            else if (rightDownCheck && rightUpCheck && !(_hangingRight && _hangingLeft))
            {
                _hangingRight = true;
                Downing(true);
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="direction">true - right, false - left</param>
        private async void Downing(bool direction)
        {
            var prevGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;
            _jumpAbility.enabled = false;

            for (float i = 0; i < downingTime; i += Time.deltaTime)
            {
                if (direction ? _input < 0 : _input > 0)
                {
                    _rb.gravityScale = prevGravity;
                    _jumpAbility.enabled = true;
                    return;
                }

                await Task.Yield();
            }

            _downingAbility.enabled = true;
            for (;;)
            {
                if (direction ? _input < 0 : _input > 0)
                {
                    _downingAbility.enabled = false;
                    _rb.gravityScale = prevGravity;
                    _jumpAbility.enabled = true;
                    return;
                }

                await Task.Yield();
            }
        }
    }
}