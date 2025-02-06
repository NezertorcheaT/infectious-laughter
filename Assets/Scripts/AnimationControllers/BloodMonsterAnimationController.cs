using UnityEngine;

namespace AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public class BloodMonsterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private Entity.Entity _originalEntity;
        private Entity.Abilities.HorizontalMovement _movement;
        private Entity.Abilities.Stun _stun;
        private static readonly int AnimatorIsWalk = Animator.StringToHash("isWalk");
        private static readonly int AnimatorIsStun = Animator.StringToHash("isStun");

        private void Start()
        {
            if (!animator) animator = gameObject.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (!_originalEntity) _originalEntity = transform.parent?.GetComponent<Entity.Entity>();
            if (!_originalEntity) _originalEntity = GetComponent<Entity.Entity>();
            if (!_stun) _movement = _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
            if (!_stun) _stun = _originalEntity.GetComponent<Entity.Abilities.Stun>();
            _movement.OnTurn += ChangeAnimator;
            _movement.OnStartedMoving += ChangeAnimator;
            _movement.OnStopped += ChangeAnimator;
            _stun.OnStunned += ChangeAnimator;
            _stun.OnUnstunned += ChangeAnimator;
        }

        private void OnDisable()
        {
            if (!_originalEntity) _originalEntity = transform.parent?.GetComponent<Entity.Entity>();
            if (!_originalEntity) _originalEntity = GetComponent<Entity.Entity>();
            if (!_stun) _movement = _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
            if (!_stun) _stun = _originalEntity.GetComponent<Entity.Abilities.Stun>();
            _movement.OnTurn -= ChangeAnimator;
            _movement.OnStartedMoving -= ChangeAnimator;
            _movement.OnStopped -= ChangeAnimator;
            _stun.OnStunned -= ChangeAnimator;
            _stun.OnUnstunned -= ChangeAnimator;
        }

        private void ChangeAnimator(bool b) => ChangeAnimator();

        private void ChangeAnimator()
        {
            animator?.SetBool(AnimatorIsWalk, _movement && _movement.TurnInFloat != 0);
            animator?.SetBool(AnimatorIsStun, _stun && _stun.IsStunned);
        }
    }
}