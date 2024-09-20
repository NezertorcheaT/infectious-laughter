using TMPro;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Walk And Jump Particle")]
    public class WalkAndJumpParticle : Ability
    {
        [SerializeField] private ParticleSystem walkParticle;
        [Range(0, 1.0f)]
        [SerializeField] float dustFormationPeriod = 0.2f;
        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private float _counter;

        private void Awake()
        {
            _collideCheck = Entity.FindExactAbilityByType<CollideCheck>();
            _playerRb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _counter += Time.deltaTime;
            if (_collideCheck.IsTouchingGround && (Mathf.Abs(_playerRb.velocity.x) > 0.0f))
            {
                if(_counter > dustFormationPeriod)
                {
                    _counter = 0;
                    walkParticle.Play();
                }
            }
        }
    }
}