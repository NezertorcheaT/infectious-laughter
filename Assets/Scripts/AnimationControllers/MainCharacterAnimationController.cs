using UnityEngine;

namespace AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public class MainCharacterAnimationController : MonoBehaviour
    {
        private Entity.Abilities.HorizontalMovement _movementController;
        private Entity.Abilities.CollideCheck _collideChecker;

        private Animator _animator;
        private static readonly int AnimatorIsWalk = Animator.StringToHash("isWalk");
        private static readonly int AnimatorJumpNow = Animator.StringToHash("jumpNow");

        private void Start()
        {
            _movementController = gameObject.GetComponent<Entity.Abilities.HorizontalMovement>();
            _collideChecker = gameObject.GetComponent<Entity.Abilities.CollideCheck>();

            _animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(AnimatorIsWalk, _movementController.TurnInFloat != 0f);
            _animator.SetBool(AnimatorJumpNow, !_collideChecker.IsTouchingGround);
        }
    }
}