using System;
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
        private Fraction _fraction;
        private Collider2D _entityMainCollider;
        private HorizontalMovement _movement;

        public IReadOnlyCollection<Entity> Hostiles { get; private set; } = Array.Empty<Entity>();

        public override void AfterInitialize()
        {
            _movement = Entity.GetComponent<HorizontalMovement>();
            _entityMainCollider = Entity.GetComponent<Collider2D>();
            _fraction = Entity.FindAvailableAbilityByInterface<Fraction>();
        }

        private void Update()
        {
            var bounds = _entityMainCollider.bounds;
            var direction = _movement.Turn ? 1f : -1f;
            var checkPosition = bounds.center +
                                new Vector3(bounds.extents.x + range / 2f, 0) * direction;
            var checkSize = new Vector2(range, bounds.size.y);
            var newBounds = new Bounds(checkPosition, checkSize);

            Helper.DrawBox(checkPosition, checkSize);
            var overlaps = _pool
                .Where(e =>
                    e.Available() &&
                    e.gameObject != Entity.gameObject &&
                    newBounds.Intersects2D(e.Bounds)
                )
                .Select(i => _pool.Fractions.GetValueOrDefault(i))
                .Where(i =>
                    i is not null &&
                    _fraction.CurrentFraction.GetRelation(i.CurrentFraction) is Relationships.Fraction.Relation
                        .Hostile
                )
                .OrderBy(i => i.CurrentFraction.Influence)
                .ThenByDescending(i => Vector2.Distance(
                    i.transform.position,
                    transform.position
                ))
                .Where(i => !(
                    i.Entity.FindExactAbilityByType<Crouching>().IsCrouching &&
                    Vector2.Distance(
                        i.transform.position,
                        transform.position
                    ) > rangeIfHidden &&
                    Physics2D.Raycast(
                        i.transform.position,
                        Vector2.down,
                        1f,
                        1 << 6
                    ).collider
                ))
                .Select(i => i.Entity)
                .Where(i =>
                {
                    var transformPosition = i.transform.position - transform.position;
                    var hit = Physics2D.Raycast(
                        newBounds.center + new Vector3(newBounds.extents.x, 0) * direction,
                        transformPosition.normalized,
                        transformPosition.magnitude,
                        1 << 0
                    );
                    return hit.collider;
                });
            Hostiles = overlaps.ToArray();
        }
    }
}