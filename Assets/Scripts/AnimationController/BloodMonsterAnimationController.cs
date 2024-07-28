using UnityEngine;

namespace AnimationController
{
    [RequireComponent(typeof(Animator))]
    public class BloodMonsterAnimationController : MonoBehaviour
    {
        private GameObject _originalEntity;
        private Animator _animator;

        private Entity.Abilities.EntityMovementHorizontalMove _movement;
        private static readonly int AnimatorIsWalk = Animator.StringToHash("isWalk");

        private void Start()
        {
            _originalEntity = transform.parent.gameObject;
            _animator = gameObject.GetComponent<Animator>();

            _movement = _originalEntity.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>();
        }

        private void Update()
        {
            _animator.SetBool(AnimatorIsWalk, _movement.TurnInFloat != 0);
        }
    }
}