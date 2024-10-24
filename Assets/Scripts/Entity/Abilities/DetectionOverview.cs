using System;
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
        private Fraction _fraction;

        public List<Entity> FriendlyEntities  = new List<Entity>();
        public List<Entity> HostileEntities  = new List<Entity>();
        private HashSet<Entity> _insideEntities = new HashSet<Entity>();

        public event Action<Entity> FriendlyEntitieDetected;
        public event Action<Entity> HostileEntitieDetected;
      
        private void OnEnable()
        {
            entityMainCollider ??= GetComponent<Collider2D>();

            _fraction = GetComponent<Fraction>();           
        }
        private void FixedUpdate()
        {
            DetectEntitiesInCircle();
        }

        private void DetectEntitiesInCircle()
        {
            var center = (Vector2)entityMainCollider.bounds.center;
            var hits = Physics2D.OverlapCircleAll(center, radius);

            var currentEntities = hits
                .Select(hit => hit.GetComponent<Entity>())
                .Where(entity => entity != null && entity.gameObject != Entity.gameObject)
                .ToHashSet();

            // Обработка входа новых сущностей в круг
            foreach (var entity in currentEntities.Except(_insideEntities))
            {
                _insideEntities.Add(entity);
                AddEntityToList(entity);
            }

            // Обработка выхода сущностей из круга
            foreach (var entity in _insideEntities.Except(currentEntities).ToList())
            {
                _insideEntities.Remove(entity);
                RemoveEntityFromList(entity);
            }
        }

        private void AddEntityToList(Entity entity)
        {
            var entityFraction = entity.FindExactAbilityByType<Fraction>();
            if (entityFraction == null) return;

            if (_fraction.type != entityFraction.type)
            {
                HostileEntities.Add(entity);
                HostileEntitieDetected?.Invoke(entity);
            }
            else
            {
                FriendlyEntities.Add(entity);
                FriendlyEntitieDetected?.Invoke(entity);
            }
        }

        private void RemoveEntityFromList(Entity entity)
        {
            FriendlyEntities.Remove(entity);
            HostileEntities.Remove(entity);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(entityMainCollider.bounds.center, radius);
        }
    }
}
