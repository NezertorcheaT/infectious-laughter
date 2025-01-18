using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Entity.AI.Neurons
{
    [AddComponentMenu("Entity/AI/Neurones/Hears")]
    public class Hears : Neurone
    {
        [Inject] private EntityPool _pool;
        [field: SerializeField] public float Range { get; private set; } = 5f;
        private Fraction _fraction;

        public override void AfterInitialize()
        {
            _fraction = Entity.GetComponent<Fraction>();
        }

        /// <summary>
        /// враги, которых слышит сейчас сущность, в порядке важности<br/>
        /// последний самый важный
        /// </summary>
        public IReadOnlyCollection<Entity> Hostiles { get; private set; } = Array.Empty<Entity>();

        private void Update()
        {
            Hostiles = _pool
                .Where(e =>
                    e.Available() &&
                    e.gameObject != Entity.gameObject &&
                    Vector2.Distance(e.transform.position, transform.position) < Range
                )
                .Select(i => (_pool.Fractions.GetValueOrDefault(i, null), _pool.Fractions.ContainsKey(i)))
                .Where(i =>
                {
                    if (!i.Item2) return false;
                    var crouching = i.Item1.Entity.FindAbilityByType<Crouching>();
                    return
                        (!crouching || !crouching.IsCrouching) &&
                        _fraction.CurrentFraction.GetRelation(i.Item1.CurrentFraction) is Relationships.Fraction
                            .Relation
                            .Hostile;
                })
                .Select(i => i.Item1)
                .OrderBy(i => i.CurrentFraction.Influence)
                .ThenByDescending(i => Vector2.Distance(
                    i.transform.position,
                    transform.position
                ))
                .Select(i => i.Entity)
                .ToArray();
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!isActiveAndEnabled) return;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
#endif
    }
}