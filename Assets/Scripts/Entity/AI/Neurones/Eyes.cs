using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Eyes")]
    public class Eyes : Neurone
    {
        [Inject] private EntityPool _pool;
        [SerializeField] private float range = 5f;
        [SerializeField] private float rangeIfHidden;
        [SerializeField, Range(0, 1)] private float rayOffset;
        private Fraction _fraction;
        private Collider2D _entityMainCollider;
        private HorizontalMovement _movement;

        private IEnumerable<Entity> BoxHostiles(Vector2 checkPosition, Vector2 checkSize) =>
            BoxHostiles(new Bounds(checkPosition, checkSize));

        private IEnumerable<Entity> BoxHostiles(Bounds bounds) => _pool
            .Where(e =>
                e.Available() &&
                e.gameObject != Entity.gameObject &&
                bounds.Intersects2D(e.Bounds)
            )
            .Select(i => i.Entity.FindExactAbilityByType<Fraction>())
            .Where(i =>
                i is not null &&
                _fraction.CurrentFraction.GetRelation(i.CurrentFraction) is Relationships.Fraction.Relation.Hostile
            )
            .OrderBy(i => i.CurrentFraction.Influence)
            .ThenByDescending(i => Vector2.Distance(
                i.Entity.CachedTransform.position,
                Entity.CachedTransform.position
            ))
            .Where(i => !(
                i.Entity.FindExactAbilityByType<Crouching>().IsCrouching &&
                Vector2.Distance(
                    i.Entity.CachedTransform.position,
                    Entity.CachedTransform.position
                ) > rangeIfHidden &&
                Physics2D.Raycast(
                    i.Entity.CachedTransform.position,
                    Vector2.down,
                    1f,
                    1 << 6
                ).collider != null
            ))
            .Select(i => i.Entity);

        public override void AfterInitialize()
        {
            _movement = gameObject.GetComponent<HorizontalMovement>();
            _entityMainCollider = gameObject.GetComponent<Collider2D>();
            _fraction = Entity.FindAvailableAbilityByInterface<Fraction>();
        }

        /// <summary>
        /// эта кароч враг для конкретной сущности <br/>
        /// Vector3 это типа последняя позиция, где видели врага
        /// </summary>
        public (Entity, Vector3?) Hostile
        {
            get
            {
                var currentRange = range;

                // коробка
                var bounds = _entityMainCollider.bounds;
                var direction = _movement.Turn ? 1f : -1f;
                var checkPosition = bounds.center +
                                    new Vector3(bounds.extents.x + currentRange / 2f, 0) * direction;
                var checkSize = new Vector2(currentRange, bounds.size.y);

                Helper.DrawBox(checkPosition, checkSize);
                var overlaps = BoxHostiles(checkPosition, checkSize).ToList();
                if (overlaps.Count == 0) return (null, null);

                var hostile = overlaps.Last();
                var hostilePosition = hostile.CachedTransform.position;

                var crouching = hostile.FindAbilityByType<Crouching>();
                if (crouching)
                    currentRange =
                        crouching.IsCrouching &&
                        Physics2D.Raycast(
                            hostilePosition,
                            Vector2.down,
                            1f,
                            1 << 6
                        ).collider != null
                            ? rangeIfHidden
                            : currentRange;

                // рейкаст для детекта стен всяких
                var hit = Physics2D.Raycast(
                    bounds.center + new Vector3(bounds.extents.x, 0) * direction,
                    (hostilePosition - transform.position).normalized,
                    currentRange,
                    1 << 0
                );

                Debug.DrawRay(
                    bounds.center + new Vector3(bounds.extents.x, 0) * direction,
                    (hostilePosition - transform.position).normalized * currentRange,
                    Color.green
                );
                if (hit.collider is not null)
                    return (hostile, hit.point);

                // рейкаст для детекта пола и потолка

                var ground = Physics2D.Raycast(
                    bounds.center - new Vector3(0, bounds.extents.y * rayOffset),
                    (hostilePosition - transform.position).normalized * currentRange,
                    1 << 0
                );

                Debug.DrawRay(
                    bounds.center - new Vector3(0, bounds.extents.y * rayOffset),
                    (hostilePosition - transform.position).normalized * currentRange,
                    Color.blue
                );
                if (ground.collider is not null)
                    return (hostile, ground.point);

                return (hostile, hostilePosition);
            }
        }
    }
}