using UnityEngine;

namespace AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public class ForestReaperAnimationController : MonoBehaviour
    {
        private GameObject _originalEntity;
        private Animator _animator;

        private Entity.Abilities.HorizontalMovement _movement;
        private Entity.Abilities.Stun _stun;
        public Entity.AI.Neurons.WoodlandReaper _brain;


        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _originalEntity ??= transform.parent.gameObject;
            _movement ??= _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
            _stun ??= _originalEntity.GetComponent<Entity.Abilities.Stun>();
            _movement.OnTurn += ChangeAnimator;
            _movement.OnStartedMoving += ChangeAnimator;
            _movement.OnStopped += ChangeAnimator;
            _stun.OnStunned += ChangeAnimator;
            _stun.OnUnstunned += ChangeAnimator;
        }

        private void OnDisable()
        {
            _originalEntity ??= transform.parent.gameObject;
            _movement ??= _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
            _stun ??= _originalEntity.GetComponent<Entity.Abilities.Stun>();
            _movement.OnTurn -= ChangeAnimator;
            _movement.OnStartedMoving -= ChangeAnimator;
            _movement.OnStopped -= ChangeAnimator;
            _stun.OnStunned -= ChangeAnimator;
            _stun.OnUnstunned -= ChangeAnimator;
        }

        private void ChangeAnimator(bool b) => ChangeAnimator();

        private void ChangeAnimator()
        {
            _animator.SetBool("AnimatorIsWalk", _movement.TurnInFloat != 0);
            _animator.SetBool("AnimatorIsStun", _stun.IsStunned);
            _animator.SetBool("AnimatorDashNow", _brain.StartDash);
            //_animator.SetBool("AnimatorIsAttack", _stun.IsStunned);
        }
    }
}