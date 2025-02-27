using UnityEngine;
using Zenject;

namespace AnimationControllers
{
    public class MainCharacterAnimationController : MonoBehaviour
    {
        [Inject] private Controls _controls;
        [SerializeField] private Animator animator;

        private Entity.Abilities.Crouching _crouching;
        private Entity.Abilities.HorizontalMovement _movementController;
        private Entity.Abilities.CollideCheck _collideChecker;

        private static readonly int AnimatorIsWalk = Animator.StringToHash("isWalk");
        private static readonly int AnimatorJumpNow = Animator.StringToHash("jumpNow");
        private static readonly int AnimatorCrouching = Animator.StringToHash("crouching");
        private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
        private static readonly int AnimatorJumpButtonPush = Animator.StringToHash("jumpButtonPush");

        private void Start()
        {
            _movementController = gameObject.GetComponent<Entity.Abilities.HorizontalMovement>();
            _collideChecker = gameObject.GetComponent<Entity.Abilities.CollideCheck>();
            _crouching = gameObject.GetComponent<Entity.Abilities.Crouching>();

            animator ??= gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            if (animator is null) return;
            animator.SetFloat(AnimatorSpeed, Mathf.Abs(_movementController.TurnInFloat * _movementController.Speed));
            animator.SetBool(AnimatorIsWalk, _movementController.TurnInFloat != 0f);
            animator.SetBool(AnimatorJumpNow, !_collideChecker.IsTouchingGround);
            animator.SetBool(AnimatorCrouching, _crouching.IsCrouching);
            animator.SetBool(AnimatorJumpButtonPush, _controls.Gameplay.Jump.IsPressed());
        }
    }
}