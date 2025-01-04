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
            _fraction = Entity.FindAvailableAbilityByInterface<Fraction>();
        }
    }
}