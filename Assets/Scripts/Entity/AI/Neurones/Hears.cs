using System.Collections.Generic;
using System.Linq;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Hears")]
    public class Hears : Neurone
    {
        [Inject] private EntityPool _pool;
        [SerializeField] private float range = 5f;
        private Fraction _fraction;

        public override void AfterInitialize()
        {
            _fraction = Entity.FindAvailableAbilityByInterface<Fraction>();
        }

        /// <summary>
        /// враги, которых слышит сейчас сущность, в порядке важности<br/>
        /// последний самый важный
        /// </summary>
        public IReadOnlyCollection<Entity> Hostiles { get; private set; }

        private void Update()
        {
            Hostiles = _pool
                .Where(e =>
                    e.Available() &&
                    e.gameObject != Entity.gameObject &&
                    Vector2.Distance(e.Entity.CachedTransform.position, Entity.CachedTransform.position) < range
                )
                .Select(i => _pool.Fractions.GetValueOrDefault(i))
                .Where(i =>
                {
                    var crouching = i?.Entity.FindAbilityByType<Crouching>();
                    return
                        i is not null &&
                        (!crouching || !crouching.IsCrouching) &&
                        _fraction.CurrentFraction.GetRelation(i.CurrentFraction) is Relationships.Fraction.Relation
                            .Hostile;
                })
                .OrderBy(i => i.CurrentFraction.Influence)
                .ThenByDescending(i => Vector2.Distance(
                    i.Entity.CachedTransform.position,
                    Entity.CachedTransform.position
                ))
                .Select(i => i.Entity)
                .ToArray();
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
#endif
    }
}