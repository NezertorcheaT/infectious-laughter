using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    //[RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Fraction))]
    [AddComponentMenu("Entity/Abilities/Detection Overview")]
    public class DetectionOverview : Ability
    {
        [SerializeField] private CircleCollider2D overLookColider;
        [SerializeField] private float viewRadius;

        private Fraction _fraction;
        private Entity _entity;
        public List<Entity> _enemies = new List<Entity>();

        private void OnEnable()
        {
            _fraction = GetComponent<Fraction>();
            _entity = GetComponent<Entity>();

            overLookColider = gameObject.AddComponent<CircleCollider2D>();
            overLookColider.isTrigger = true;
            overLookColider.radius = viewRadius;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(TryGetComponent<Entity>(out Entity entity) 
                && _entity != entity 
                )//&& TryGetComponent<Fraction>(out Fraction fraction))
            {
                //if (_fraction.type == fraction.type && !_enemies.Contains(entity))
                {
                    _enemies.Add(entity);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (TryGetComponent<Entity>(out Entity entity))
            {
                if (_enemies.Contains(entity)) _enemies.Remove(entity);
            }
        }
    }
}