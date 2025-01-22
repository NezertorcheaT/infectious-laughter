using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using Entity.Relationships.Fractions;
using UnityEngine;

namespace PropsImpact
{
    public class HomingDartPrefab : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float stunTime;
        [SerializeField] private float speed = 1;
        private Transform _selfTransform;
        private EntityCacher _target;
        private Stun _stun;
        private bool _initialized;
        private bool _destroyed;

        private async void Start()
        {
            _selfTransform = gameObject.transform;
            while (true)
            {
                if (_destroyed) return;
                var hits = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);

                foreach (var hit in hits)
                {
                    if (!hit.gameObject.TryGetComponent(out _stun)) continue;
                    if (!hit.gameObject.TryGetComponent(out _target)) continue;
                    if (!hit.gameObject.TryGetComponent(out Fraction fraction)) continue;
                    if (fraction.CurrentFraction.GetRelation(new PlayerFraction()) is not Entity.Relationships.Fraction
                            .Relation.Hostile) continue;
                    _ = GoToTarget();
                    return;
                }

                if (_destroyed) return;
                await Task.Delay(100);
            }
        }

        public void Initialize(float speed)
        {
            if (_initialized) return;
            this.speed = speed;
            _initialized = true;
        }

        private async Task GoToTarget()
        {
            while (true)
            {
                if (_destroyed) return;
                var targetCenter = _target.Bounds.center;
                _selfTransform.rotation = Quaternion.Euler(
                    _selfTransform.rotation.eulerAngles.x,
                    _selfTransform.rotation.eulerAngles.y,
                    Mathf.Atan2(
                        targetCenter.y - _selfTransform.position.y,
                        targetCenter.x - _selfTransform.position.x
                    ) * Mathf.Rad2Deg - 90
                );
                _selfTransform.position += (targetCenter - _selfTransform.position).normalized * speed;
                if (Vector3.Distance(_selfTransform.position, targetCenter) < 0.05) Die();
                if (_destroyed) return;
                await UniTask.WaitForFixedUpdate();
            }
        }

        private void Die()
        {
            _destroyed = true;
            _ = _stun.Perform(stunTime);
            Destroy(gameObject);
        }

        private void OnDestroy() => _destroyed = true;
    }
}