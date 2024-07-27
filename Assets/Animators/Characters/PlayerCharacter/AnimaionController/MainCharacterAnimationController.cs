using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationControllers
{
    public class MainCharacterAnimationController : MonoBehaviour
    {
        private Entity.Abilities.EntityMovementHorizontalMove _movementController;
        private Entity.Abilities.CollideCheck _collideChecker;

        private Animator _animator;

        private void Start()
        {
            _movementController = gameObject.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>();
            _collideChecker = gameObject.GetComponent<Entity.Abilities.CollideCheck>();
            
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool("isWalk", _movementController.TurnInFloat != 0f);
            _animator.SetBool("jumpNow", !_collideChecker.IsTouchingGround);
        }
    }
}