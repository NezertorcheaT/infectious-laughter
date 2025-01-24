using System;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurons
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
        private long _approachStopwatch;
        private long _approachWaitDelayTicks;

        private float ApproachWaitDelay
        {
            get => approachWaitDelay;
            set
            {
                approachWaitDelay = value;
                _approachWaitDelayTicks = TimeSpan.FromSeconds(ApproachWaitDelay).Ticks;
            }
        }

        public override void AfterInitialize()
        {
            _approachWaitDelayTicks = TimeSpan.FromSeconds(ApproachWaitDelay).Ticks;
            _movement = Entity.FindAbilityByType<HorizontalMovement>();
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

            _currentTarget = null;
            if (eyes && eyes.Hostiles.Count != 0)
                _currentTarget = eyes.Hostiles.LastOrDefault();
            if (hears && hears.Hostiles.Count != 0)
                _currentTarget = hears.Hostiles.LastOrDefault();
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
                if (!_movement || !_movement.Available())
                {
                    _movement?.Move(0);
                    continue;
                }

                if (_approaching)
                {
                    if ((transform.position - _lastSeen).magnitude < approachMinDistance)
                    {
                        _movement.Move(0);
                        _approaching = false;
                        await UniTask.WhenAny(
                            UniTask.WaitForSeconds(ApproachWaitDelay),
                            UniTask.WaitUntil(() => _currentTarget)
                        );
                    }

                    var velocity = (_lastSeen - transform.position).x;
                    if (velocity != 0) _movement.Move(velocity / Mathf.Abs(velocity));
                    if (
                        (_lastSeen - transform.position).magnitude < eyes.Range * 2 &&
                        Stopwatch.GetTimestamp() - _approachStopwatch > _approachWaitDelayTicks
                    ) _approaching = false;
                }

                if (!_currentTarget)
                {
                    if (!_approaching) _movement.Move(_partrollingDirection ? 1 : -1);
                    continue;
                }

                _approaching = true;
                _approachStopwatch = Stopwatch.GetTimestamp();
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