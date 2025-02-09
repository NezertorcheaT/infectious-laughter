using SoundSystem;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Walk And Jump Particle")]
    public class WalkAndJumpParticle : Ability
    {
        [SerializeField] private ParticleSystem leftWalkParticle;
        [SerializeField] private ParticleSystem rightWalkParticle;
        [SerializeField] [Range(0, 1.0f)] private float dustFormationPeriod = 0.2f;

        [SerializeField] private AudioSource stepSound;
        [SerializeField] private float minPitchValue;
        [SerializeField] private float maxPitchValue;
        private RandomPitchDeliver _soundDeliver;

        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;
        private float _counter;

        private void Awake()
        {
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
            _playerRb = GetComponent<Rigidbody2D>();
            _soundDeliver = GetComponent<RandomPitchDeliver>();
        }

        private void Update()
        {
            _counter += Time.deltaTime;
            if (!_collideCheck.IsTouchingGround || Mathf.Abs(_playerRb.velocity.x) <= 0.0f) return;
            if (_counter <= dustFormationPeriod) return;
            _counter = 0;
            if (_playerRb.velocity.x > 0)
            {
                rightWalkParticle.Play();

                //_soundDeliver.DeliveClip(stepSound, minPitchValue, maxPitchValue); проверялся звук шагов, но его нужно делать в связке с анимацией персонажа
            }
            else
            {
                leftWalkParticle.Play();

                //_soundDeliver.DeliveClip(stepSound, minPitchValue, maxPitchValue);
            }
        }
    }
}