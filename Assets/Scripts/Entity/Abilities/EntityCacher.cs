using CustomHelper;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Casher")]
    public class EntityCacher : Ability
    {
        [Inject] private EntityPool _pool;
        [SerializeField] private bool hideBox = true;
        [SerializeField, HideIf("hideBox")] private Vector2 offset;
        [SerializeField, HideIf("hideBox")] private Vector2 scale = new(1, 1);
        public Bounds Bounds => new(transform.position + offset.ToVector3(), scale);

        private void Start()
        {
            _pool.Add(this);
        }

        private void OnDestroy()
        {
            _pool.Remove(this);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (hideBox) return;
            Helper.DrawBox(gameObject.transform.position + offset.ToVector3(), scale);
        }
#endif
    }
}