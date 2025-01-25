using System.Linq;
using Entity.Controllers;
using UnityEngine;

namespace AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public class ForestReaperAnimationController : MonoBehaviour
    {
        private static readonly int AnimatorDashNow = Animator.StringToHash("AnimatorDashNow");
        private static readonly int AnimatorIsStun = Animator.StringToHash("AnimatorIsStun");
        private static readonly int AnimatorIsWalk = Animator.StringToHash("AnimatorIsWalk");

        private GameObject _originalEntity;
        private Animator _animator;

        private Entity.Abilities.HorizontalMovement _movement;
        private Entity.Abilities.Stun _stun;
        private Entity.AI.Neurons.WoodlandReaper _brain;
        private ControllerAI _controller;

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Awake()
        {
            _originalEntity ??= transform.parent.gameObject;
            _controller = _originalEntity.GetComponent<ControllerAI>();
            _controller.OnInitializationComplete += ControllerOnInitializationComplete;
        }

        private void OnEnable()
        {
            _movement ??= _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
            _stun ??= _originalEntity.GetComponent<Entity.Abilities.Stun>();
            _movement.OnTurn += ChangeAnimator;
            _movement.OnStartedMoving += ChangeAnimator;
            _movement.OnStopped += ChangeAnimator;
            _stun.OnStunned += ChangeAnimator;
            _stun.OnUnstunned += ChangeAnimator;
        }

        private void ControllerOnInitializationComplete()
        {
            _brain = _controller
                    .CurrentBrain
                    .Neurones
                    .FirstOrDefault(i => i is Entity.AI.Neurons.WoodlandReaper)
                as Entity.AI.Neurons.WoodlandReaper;
            _controller.OnInitializationComplete -= ControllerOnInitializationComplete;
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
            _animator?.SetBool(AnimatorIsWalk, _movement && _movement.TurnInFloat != 0);
            _animator?.SetBool(AnimatorIsStun, _stun && _stun.IsStunned);
            _animator?.SetBool(AnimatorDashNow, _brain && _brain.StartDash);
        }
    }
}