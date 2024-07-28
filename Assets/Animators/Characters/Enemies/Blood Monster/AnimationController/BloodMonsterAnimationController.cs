using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class BloodMonsterAnimationController : MonoBehaviour
{
    private GameObject _originalEntity;
    private Animator _animator;

    private Entity.Abilities.EntityMovementHorizontalMove _movement;

    private void Start()
    {
        _originalEntity = transform.parent.gameObject;
        _animator = gameObject.GetComponent<Animator>();
        
        _movement = _originalEntity.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>();
    }

    private void Update()
    {
        _animator.SetBool("isWalk", _movement.TurnInFloat != 0);

    }

}
