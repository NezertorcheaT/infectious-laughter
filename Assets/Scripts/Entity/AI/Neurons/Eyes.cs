using System;
using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Entity.AI.Neurons
{
    [AddComponentMenu("Entity/AI/Neurones/Eyes")]
    public class Eyes : Neurone
    {
        [Inject] private EntityPool _pool;
        [field: SerializeField, Min(0)] public float Range { get; private set; } = 5f;
        private Fraction _fraction;
        private Collider2D _entityMainCollider;
        private HorizontalMovement _movement;
        private IReadOnlyCollection<Entity> _hostiles = Array.Empty<Entity>();
        private static readonly Entity[] EmptyEntities = Array.Empty<Entity>();

        public IReadOnlyCollection<Entity> Hostiles => isActiveAndEnabled ? _hostiles : EmptyEntities;

        public override void AfterInitialize()
        {
            _movement = Entity.GetComponent<HorizontalMovement>();
            _entityMainCollider = Entity.GetComponent<Collider2D>();
            _fraction = Entity.GetComponent<Fraction>();
        }

        private void Update()
        {
            var bounds = _entityMainCollider.bounds;
            var direction = _movement.Turn ? 1f : -1f;
            var checkPosition = bounds.center +
                                new Vector3(bounds.extents.x + Range / 2f, 0) * direction;
            var checkSize = new Vector2(Range, bounds.size.y);
            var newBounds = new Bounds(checkPosition, checkSize);

            Helper.DrawBox(checkPosition, checkSize);
            var overlaps = _pool
                .Where(e =>
                    e.Available() &&
                    e.gameObject != Entity.gameObject &&
                    newBounds.Intersects2D(e.Bounds)
                )
                .Select(i => (_pool.Fractions.GetValueOrDefault(i, null), _pool.Fractions.ContainsKey(i)))
                .Where(i => i.Item2 &&
                            _fraction.CurrentFraction.GetRelation(i.Item1.CurrentFraction) is Relationships.Fraction
                                .Relation
                                .Hostile)
                .Select(i => i.Item1)
                .OrderBy(i => i.CurrentFraction.Influence)
                .ThenByDescending(i => Vector2.Distance(
                    i.transform.position,
                    transform.position
                ))
                .Where(i =>
                {
                    var crouching = i.Entity.FindExactAbilityByType<Crouching>();
                    if (!crouching) return true;
                    return !(
                        crouching.IsCrouching &&
                        Physics2D.Raycast(
                            i.transform.position,
                            Vector2.down,
                            1f,
                            1 << 6
                        ).collider
                    );
                })
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
                    return !hit.collider;
                });
            _hostiles = overlaps.ToArray();
        }
    }
}