using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Entity.Relationships;
using Installers;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Hostile Detection")]
    [RequireComponent(typeof(Fraction))]
    [RequireComponent(typeof(Collider2D))]
    public class HostileDetection : Ability
    {
        [Inject] private PlayerInstallation _playerInstallation;
        [SerializeField] private Collider2D entityMainCollider;
        public float range = 5f;
        public float rangeIfHidden = 2.5f;
        public bool direction = false;
        private Relationships.Fraction _fraction;
        private Crouching _playerMovementCrouch;
        private Fraction _playerEntityFraction;


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
                _playerMovementCrouch ??= _playerInstallation.Entity.FindAbilityByType<Crouching>();
                _playerEntityFraction ??= _playerInstallation.Entity.FindAbilityByType<Fraction>();

                var playerPosition = _playerInstallation.Entity.CachedTransform.position;
                var playerFraction = _playerEntityFraction.CurrentFraction;
                var playerIsHidden = _playerMovementCrouch.IsCrouching &&
                                     Physics2D.Raycast(playerPosition, Vector2.down, 1f, 1 << 6).collider != null;
                var currentRange = playerIsHidden ? rangeIfHidden : range;

                // коробка
                var bounds = entityMainCollider.bounds;
                var checkPosition = bounds.center +
                                    new Vector3(bounds.extents.x + currentRange / 2f, 0) * (direction ? 1f : -1f);
                var checkSize = new Vector2(currentRange, bounds.size.y);

                Helper.DrawBox(checkPosition, checkSize);
                var overlaps = Physics2D.OverlapBoxAll(
                    checkPosition,
                    checkSize,
                    0,
                    1 << 3).Select(i => i.gameObject.GetComponent<Entity>());

                // типа если в коробке пусто, значт он игрока рейкастом не увидит никак
                if (!overlaps.Contains(_playerInstallation.Entity, new EntityEqualityComparer()))
                    return (null, playerPosition);

                // рейкаст для детекта стен всяких
                var hit = Physics2D.Raycast(
                    bounds.center + new Vector3(bounds.extents.x, 0) * (direction ? 1f : -1f),
                    (playerPosition - transform.position).normalized,
                    currentRange,
                    1 << 0
                );
                Debug.DrawRay(
                    bounds.center + new Vector3(bounds.extents.x, 0) * (direction ? 1f : -1f),
                    (playerPosition - transform.position).normalized,
                    Color.green,
                    currentRange
                );
                if (hit.collider is not null)
                    return (null, hit.point);

                return (
                    _fraction.GetRelation(playerFraction) is Relationships.Fraction.Relation.Hostile
                        ? _playerInstallation.Entity
                        : null,
                    null
                );
            }
        }

        private class EntityEqualityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity b1, Entity b2)
            {
                if (ReferenceEquals(b1, b2))
                    return true;

                if (b2 is null || b1 is null)
                    return false;

                return b1.gameObject == b2.gameObject;
            }

            public int GetHashCode(Entity entity) => entity.GetHashCode();
        }
    }
}