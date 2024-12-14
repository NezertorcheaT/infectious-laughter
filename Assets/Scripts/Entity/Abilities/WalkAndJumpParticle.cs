using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Walk And Jump Particle")]
    public class WalkAndJumpParticle : Ability
    {
        [SerializeField] private ParticleSystem walkParticle;
        [SerializeField] [Range(0, 1.0f)] private float dustFormationPeriod = 0.2f;
        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private float _counter;

        private void Awake()
        {
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
            _playerRb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _counter += Time.deltaTime;
            if (!_collideCheck.IsTouchingGround || Mathf.Abs(_playerRb.velocity.x) <= 0.0f) return;
            if (_counter <= dustFormationPeriod) return;
            _counter = 0;
            walkParticle.Play();
        }
    }
}