using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Blood Monster")]
    public class BloodMonsterWalk : Neurone
    {
        [SerializeField] private Eyes eyes;
        [SerializeField] private Hears hears;
        [SerializeField, Min(0)] private float thoughtDelay = 0.1f;
        [SerializeField, Min(0)] private float patrollingReverseDelay = 5;
        [SerializeField, Min(0)] private float approachMinDistance = 1;
        [SerializeField, Min(0)] private float approachWaitDelay = 5;
        private HorizontalMovement _movement;
        private Entity _currentTarget;
        private bool _approaching;
        private bool _partrollingDirection;
        private bool _destroyed;
        private Vector3 _lastSeen;

        public override void AfterInitialize()
        {
            _movement = Entity.FindAvailableAbilityByInterface<HorizontalMovement>();
            _ = PatrollingSwitch();
            _ = FragmentedUpdate();
        }


        private async UniTaskVoid PatrollingSwitch()
        {
            while (true)
            {
                if (patrollingReverseDelay == 0) return;
                await UniTask.WaitForSeconds(patrollingReverseDelay);
                if (_destroyed) return;
                if (!Available() || _approaching) continue;
                _partrollingDirection = !_partrollingDirection;
            }
        }

        private void Update()
        {
            if (!Available()) return;
            if (!_movement || !_movement.Available()) return;

            _currentTarget = null;
            if (eyes && eyes.Hostiles.Count != 0)
                _currentTarget = eyes.Hostiles.LastOrDefault();
            if (hears && hears.Hostiles.Count != 0)
                _currentTarget ??= hears.Hostiles.LastOrDefault();
            if (_currentTarget)
                _lastSeen = _currentTarget.transform.position;
        }

        private async UniTaskVoid FragmentedUpdate()
        {
            while (true)
            {
                if (thoughtDelay == 0) return;
                await UniTask.WaitForSeconds(thoughtDelay);
                if (_destroyed) return;

                if (_approaching)
                {
                    if ((transform.position - _lastSeen).magnitude < approachMinDistance)
                    {
                        _approaching = false;
                        await UniTask.WhenAny(
                            UniTask.WaitForSeconds(approachWaitDelay),
                            UniTask.WaitUntil(() => _currentTarget)
                        );
                    }

                    var velocity = (_lastSeen - transform.position).x;
                    if (velocity != 0) _movement.Move(velocity / Mathf.Abs(velocity));
                }

                if (!_currentTarget)
                {
                    if (!_approaching) _movement.Move(_partrollingDirection ? 1 : -1);
                    continue;
                }

                _approaching = true;
            }
        }

        private void OnDrawGizmos()
        {
            if (_approaching)
                Gizmos.DrawSphere(_lastSeen, 1);
        }

        private void OnDestroy()
        {
            _destroyed = true;
        }
    }
}