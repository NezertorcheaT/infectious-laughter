using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Detection Overview")]
    [RequireComponent(typeof(Fraction))]
    [RequireComponent(typeof(Collider2D))]
    public class DetectionOverview : Ability
    {
        [Inject] private EntityPool _pool;
        [SerializeField] private Collider2D entityMainCollider;
        [SerializeField, Range(0, 1)] private float rayOffset;
        public float radius = 5f;
        public bool direction;
        private Relationships.Fraction _fraction;

        public List<Entity> FriendlyEntities { get; private set; } = new List<Entity>();
        public List<Entity> HostileEntities { get; private set; } = new List<Entity>();

        private void DetectEntitiesInCircle(Vector2 center, float radius)
        {
            FriendlyEntities.Clear();
            HostileEntities.Clear();

            var hits = Physics2D.OverlapCircleAll(center, radius);

            foreach (var hit in hits)
            {
                if (hit.gameObject == Entity.gameObject) continue;

                var entity = hit.GetComponent<Entity>();
                if (entity == null) continue;

                var entityFraction = entity.FindExactAbilityByType<Fraction>();
                if (entityFraction == null) continue;

                if (_fraction.GetRelation(entityFraction.CurrentFraction) == Relationships.Fraction.Relation.Hostile)
                {
                    HostileEntities.Add(entity);
                }
                else
                {
                    FriendlyEntities.Add(entity);
                }
            }
        }

        /// <summary>
        /// Возвращает ближайшего враждебного существа и его последнюю известную позицию.
        /// </summary>
        public (Entity, Vector3?) Hostile
        {
            get
            {
                entityMainCollider ??= gameObject.GetComponent<Collider2D>();
                _fraction ??= Entity.FindAbilityByType<Fraction>().CurrentFraction;

                var center = (Vector2)entityMainCollider.bounds.center;
                DetectEntitiesInCircle(center, radius);

                if (HostileEntities.Count == 0) return (null, null);

                var hostile = HostileEntities.OrderByDescending(h =>
                    Vector2.Distance(h.CachedTransform.position, Entity.CachedTransform.position)).First();

                var hostilePosition = hostile.CachedTransform.position;

                // Проверка, не скрыта ли цель.
                var crouching = hostile.FindAbilityByType<Crouching>();
                if (crouching && crouching.IsCrouching)
                {
                    var hidden = Physics2D.Raycast(
                        hostilePosition, Vector2.down, 1f, 1 << 6).collider != null;
                    if (hidden) return (hostile, null);
                }

                // Проверка наличия стены между объектами.
                var hit = Physics2D.Raycast(
                    center,
                    (hostilePosition - transform.position).normalized,
                    radius,
                    1 << 0
                );

                Debug.DrawRay(
                    center,
                    (hostilePosition - transform.position).normalized * radius,
                    Color.green
                );

                if (hit.collider != null) return (hostile, hit.point);

                return (hostile, hostilePosition);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(entityMainCollider.bounds.center, radius);
        }
    }
}
