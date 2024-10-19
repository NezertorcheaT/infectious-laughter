using Entity.Abilities;
using UnityEngine;
using System.Threading.Tasks;

namespace PropsImpact
{
    public class RockBehaviour : MonoBehaviour
    {
        [SerializeField] private ParticleSystem impactParticle;
        [SerializeField] private Collider2D Collider2D;
        [SerializeField] private TrailRenderer trailRenderer;

        [SerializeField] private float destroyDelay;

        private int damage;
        private int stunTime;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            impactParticle.Play();

            if (collision.transform.tag == "Player") return;

            if (collision.transform.TryGetComponent<Stun>(out Stun stunAbility))
            {
                PerformStunAsync(stunAbility);
            }
            if (collision.transform.TryGetComponent<Hp>(out Hp enemyHp))
            {
                enemyHp.AddDamage(damage);
                DestroyIvent();
            }
        }

        private async Task PerformStunAsync(Stun stunAbility)
        {
            await stunAbility.Perform(stunTime);
        }

        private void DestroyIvent()
        {
            SetEnabled(false);

            impactParticle.startSize = 0.9f;
            impactParticle.Play();

            Destroy(gameObject, 0.3f);
        }

        private void SetEnabled(bool enabled)
        {
            Collider2D.enabled = enabled;
            trailRenderer.enabled = enabled;
        }

        public void Initialize(int _damage, int _stunTime)
        {
            Destroy(gameObject, destroyDelay);

            damage = _damage;
            stunTime = _stunTime;
        }

    }
}