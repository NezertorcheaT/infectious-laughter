using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurons
{
    [AddComponentMenu("Entity/AI/Neurones/Woodland Reaper")]
    public class WoodlandReaper : Neurone
    {
        public bool DashNow { get; private set; }
        public bool StartDash { get; private set; }

        [SerializeField] private Eyes eyes;
        [SerializeField] private Hears hears;
        [SerializeField, Min(0)] private float thoughtDelay = 0.1f;
        [SerializeField, Min(0)] private float patrollingReverseDelay = 5;
        [SerializeField, Min(0)] private float approachMinDistance = 1;
        [SerializeField, Min(0)] private float approachWaitDelay = 5;
        [SerializeField, Min(1)] private float distanceThresholdBeforeDash = 10;
        [SerializeField, Min(1)] private int dashForceMultiplier = 3;
        [SerializeField, Min(0.1f)] private float waitBeforeDash = 0.1f;
        [SerializeField, Min(0.1f)] private float dashCooldown = 7f;
        [SerializeField, Min(0.05f)] private float dashTime;
        [SerializeField] private int speedDuringPreparations;
        private HorizontalMovement _movement;
        private Entity _currentTarget;
        private bool _canDash = true;
        private bool _approaching;
        private bool _partrollingDirection;
        private bool _destroyed;
        private bool _waitForDash;
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
            {
                _currentTarget = eyes.Hostiles.LastOrDefault();
                if (
                    _currentTarget &&
                    !_waitForDash &&
                    Vector3.Distance(
                        eyes.transform.position,
                        _currentTarget.transform.position
                    ) <= distanceThresholdBeforeDash
                ) _waitForDash = true;
            }

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

                    if (_waitForDash && _canDash) StartCoroutine(WaitForEndDash());

                    var velocity = (_lastSeen - transform.position).x;
                    if (velocity != 0)
                        _movement.Move((velocity / Mathf.Abs(velocity)) * (DashNow ? dashForceMultiplier : 1f));

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
            Gizmos.DrawWireSphere(eyes.transform.position, distanceThresholdBeforeDash);
        }

        private void OnDestroy() => _destroyed = true;

        private IEnumerator WaitForEndDash()
        {
            _waitForDash = false;
            _canDash = false;
            var x = dashForceMultiplier;
            dashForceMultiplier = speedDuringPreparations;
            DashNow = true;
            yield return new WaitForSeconds(waitBeforeDash);
            StartDash = true;
            dashForceMultiplier = x;
            yield return new WaitForSeconds(dashTime);
            StartDash = false;
            DashNow = false;
            yield return new WaitForSeconds(dashCooldown);
            _canDash = true;
        }
    }
}