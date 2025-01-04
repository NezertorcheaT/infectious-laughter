using System.Linq;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Basic Eye")]
    public class BasicEye : Neurone
    {
        [Inject] private EntityPool _pool;

        [SerializeField] private float range = 5f;
        private Fraction _fractionAbility;

        public bool IsSeeing { get; private set; }

        public override void AfterInitialize()
        {
            _fractionAbility = Entity.FindAvailableAbilityByInterface<Fraction>();
        }

        private void FixedUpdate()
        {
            IsSeeing = _pool
                .Where(e => e.gameObject != Entity.gameObject)
                .Select(i => i.Entity.FindExactAbilityByType<Fraction>())
                .Where(i => i is not null &&
                            _fractionAbility.CurrentFraction.GetRelation(i.CurrentFraction) is Relationships.Fraction
                                .Relation.Hostile)
                .Any(i => (i.Entity.CachedTransform.position - Entity.CachedTransform.position).magnitude < range);
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
#endif
    }
}