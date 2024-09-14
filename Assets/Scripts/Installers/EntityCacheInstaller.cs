using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private HashSet<EntityCacher> _entities;

        public EntityPool()
        {
            _entities = new HashSet<EntityCacher>(2, new EntityComparer());
        }

        public void Add(EntityCacher entity)
        {
            if (entity is null) return;
            _entities.Add(entity);
        }

        public void Remove(EntityCacher entity)
        {
            _entities.Remove(entity);
        }

        public IEnumerator<EntityCacher> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class EntityComparer : IEqualityComparer<EntityCacher>
        {
            public bool Equals(EntityCacher x, EntityCacher y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.gameObject, y.gameObject);
            }

            public int GetHashCode(EntityCacher obj) => obj.gameObject != null ? obj.gameObject.GetHashCode() : 0;
        }
    }
}