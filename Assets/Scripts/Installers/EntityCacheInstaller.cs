using System.Collections;
using System.Collections.Generic;
using Entity;
using Entity.Abilities;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Entity Cache")]
    public class EntityCacheInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var pool = new EntityPool();
            Container.Bind<EntityPool>().FromInstance(pool).AsSingle();
        }
    }
}

namespace Entity
{
    public class EntityPool : IEnumerable<EntityCacher>
    {
        public IReadOnlyDictionary<EntityCacher, Fraction> Fractions => _fractions;

        private readonly HashSet<EntityCacher> _entities = new(2, new EntityComparer());
        private readonly Dictionary<EntityCacher, Fraction> _fractions = new(2, new EntityComparer());

        public void Add(EntityCacher entity)
        {
            if (entity is null) return;
            _entities.Add(entity);
            if (_fractions.ContainsKey(entity) || !entity.gameObject.TryGetComponent<Fraction>(out var fr)) return;
            _fractions.Add(entity, fr);
        }

        public void Remove(EntityCacher entity)
        {
            if (entity is null) return;
            _entities.Remove(entity);
            if (!_fractions.ContainsKey(entity)) return;
            _fractions.Remove(entity);
        }

        public IEnumerator<EntityCacher> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class EntityComparer : IEqualityComparer<Ability>
        {
            public bool Equals(Ability x, Ability y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.gameObject, y.gameObject);
            }

            public int GetHashCode(Ability obj) => obj.gameObject ? obj.gameObject.GetHashCode() : 0;
        }
    }
}