using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Hostile Detection")]
    [RequireComponent(typeof(Fraction))]
    [RequireComponent(typeof(Collider2D))]
    public class HostileDetection : Ability
    {
        [SerializeField] private Collider2D entityMainCollider;
        [SerializeField, Range(0, 1)] private float rayOffset;
        public float range = 5f;
        public float rangeIfHidden = 2.5f;
        public bool direction;
        private Relationships.Fraction _fraction;

        private List<Entity> BoxHostiles(Vector2 checkPosition, Vector2 checkSize)
        {
            return Physics2D.OverlapBoxAll(
                        checkPosition,
                        checkSize,
                        0,
                        1 << 3)
                    .Select(i => i.gameObject.GetComponent<Entity>().GetComponent<Fraction>())
                    .Where(i => i is not null &&
                                _fraction.GetRelation(i.CurrentFraction) is Relationships.Fraction.Relation.Hostile)
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
                        Physics2D.Raycast(i.Entity.CachedTransform.position, Vector2.down, 1f, 1 << 6).collider !=
                        null
                    ))
                    .Select(i => i.Entity)
                    .ToList()
                ;
        }

        /// <summary>
        /// эта кароч враг для конкретной сущности <br/>
        /// Vector3 это типа последняя позиция, где видели врага
        /// </summary>
        public (Entity, Vector3?) Hostile
        {
            get
            {
                entityMainCollider ??= gameObject.GetComponent<Collider2D>();
                _fraction ??= Entity.FindAbilityByType<Fraction>().CurrentFraction;
                var currentRange = range;

                // коробка
                var bounds = entityMainCollider.bounds;
                var checkPosition = bounds.center +
                                    new Vector3(bounds.extents.x + currentRange / 2f, 0) * (direction ? 1f : -1f);
                var checkSize = new Vector2(currentRange, bounds.size.y);

                Helper.DrawBox(checkPosition, checkSize);
                var overlaps = BoxHostiles(checkPosition, checkSize);
                if (overlaps.Count == 0)
                    return (null, null);
                var hostile = overlaps.Last();
                var hostilePosition = hostile.CachedTransform.position;

                var crouching = hostile.FindAbilityByType<Crouching>();
                if (crouching)
                {
                    var isHidden = crouching.IsCrouching &&
                                   Physics2D.Raycast(hostilePosition, Vector2.down, 1f, 1 << 6).collider != null;
                    currentRange = isHidden ? rangeIfHidden : currentRange;
                }

                // рейкаст для детекта стен всяких
                var hit = Physics2D.Raycast(
                    bounds.center + new Vector3(bounds.extents.x, 0) * (direction ? 1f : -1f),
                    (hostilePosition - transform.position).normalized,
                    currentRange,
                    1 << 0
                );

                Debug.DrawRay(
                    bounds.center + new Vector3(bounds.extents.x, 0) * (direction ? 1f : -1f),
                    (hostilePosition - transform.position).normalized * currentRange,
                    Color.green
                );
                if (hit.collider is not null)
                    return (null, hit.point);

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
                    return (null, ground.point);

                return (hostile, hostilePosition);
            }
        }
    }
}