using UnityEngine;

namespace AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public class BloodMonsterAnimationController : MonoBehaviour
    {
        private GameObject _originalEntity;
        private Animator _animator;

        private Entity.Abilities.HorizontalMovement _movement;
        private static readonly int AnimatorIsWalk = Animator.StringToHash("isWalk");

        private void Start()
        {
            _originalEntity = transform.parent.gameObject;
            _animator = gameObject.GetComponent<Animator>();

            _movement = _originalEntity.GetComponent<Entity.Abilities.HorizontalMovement>();
        }

        private void Update()
        {
            _animator.SetBool(AnimatorIsWalk, _movement.TurnInFloat != 0);
        }
    }
}