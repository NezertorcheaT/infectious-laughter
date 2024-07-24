using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    public class MovementCameraFollowPointAbility : Ability
    {
        [SerializeField] private Transform followPoint;

        [SerializeField] private float maxRange;
        private bool _locked = true;
        [SerializeField] private float speed;

        [SerializeField] private EntityMovementHorizontalMove MovementAbility;
        [SerializeField] private PlayerJumpAbility JumpAbility;

        private Vector2 _chachedPosition;


        public void MovePoint(float turn)
        {
            if(_locked) return;
            if( (gameObject.transform.position.x - followPoint.position.x) >= maxRange == false && turn < 0)
            {
                followPoint.Translate(-speed * Time.deltaTime, 0, 0);
            }
            if((gameObject.transform.position.x - followPoint.position.x) <= -maxRange == false && turn > 0)
            {
                followPoint.Translate(speed * Time.deltaTime, 0, 0);
            }
        }

        public void ChangeLock()
        {
            _chachedPosition = followPoint.position;
            _locked = !_locked;
            followPoint.position = gameObject.transform.position;

            JumpAbility.enabled = _locked;
            MovementAbility.enabled = _locked;
            
        }

    }
}