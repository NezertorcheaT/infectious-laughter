using Entity.Abilities;
using UnityEngine;

namespace PropsImpact
{
    public class RockBehaviour : MonoBehaviour
    {
        [SerializeField] private ParticleSystem impactParticle;
        [SerializeField] private Collider2D collider;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private float destroyDelay;

        private int _damage;
        private int _stunTime;
        private bool _initialized;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            impactParticle.Play();

            if (collision.transform.CompareTag("Player")) return;

            if (collision.transform.TryGetComponent<Stun>(out var stunAbility))
                stunAbility.Perform(_stunTime);

            if (!collision.transform.TryGetComponent<Hp>(out var enemyHp)) return;

            collision.rigidbody.velocity = Vector2.zero;
            collision.otherRigidbody.velocity = Vector2.zero;

            enemyHp.AddDamage(_damage);
            SetEnabled(false);

            impactParticle.startSize = 0.9f;
            impactParticle.Play();

            Destroy(gameObject, 0.3f);
        }

        private void SetEnabled(bool enabled)
        {
            collider.enabled = enabled;
            trailRenderer.enabled = enabled;
        }

        public void Initialize(int damage, int stunTime)
        {
            if (_initialized) return;
            _initialized = true;
            Destroy(gameObject, destroyDelay);

            _damage = damage;
            _stunTime = stunTime;
        }
    }
}