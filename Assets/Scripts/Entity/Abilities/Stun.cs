using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(HorizontalMovement))]
    [AddComponentMenu("Entity/Abilities/Stun")]
    public class Stun : Ability
    {
        [SerializeField, Min(0)] private float stunTime;
        private Ability _moveAbility;
        private Ability _jumpAbility;

        public event Action OnStunned;
        public event Action OnUnstunned;

        public bool IsStunned { get; private set; }

        private void Start()
        {
            _jumpAbility = Entity.FindAvailableAbilityByInterface<IJumpableAbility>() as Ability;
            _moveAbility = Entity.FindAbilityByType<HorizontalMovement>();
        }

        public async Task Perform(float time)
        {
            if (!Available()) return;
            if (stunTime != 0)
            {
                stunTime = Mathf.Max(time, stunTime);
                await UniTask.WaitUntil(() => stunTime <= 0);
                return;
            }

            IsStunned = true;
            OnStunned?.Invoke();
            _jumpAbility.enabled = false;
            _moveAbility.enabled = false;

            for (stunTime = time; stunTime > 0; stunTime -= Time.fixedDeltaTime)
            {
                _jumpAbility.enabled = false;
                _moveAbility.enabled = false;
                await UniTask.WaitForFixedUpdate();
            }

            stunTime = 0;
            _jumpAbility.enabled = true;
            _moveAbility.enabled = true;
            IsStunned = false;
            OnUnstunned?.Invoke();
        }
    }
}