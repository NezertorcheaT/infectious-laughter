using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Walk And Jump Particle")]
    public class WalkAndJumpParticle : Ability
    {
        [SerializeField] private ParticleSystem walkParticle;
        private Rigidbody2D _playerRb;
        private CollideCheck _collideCheck;

        private void Start()
        {
            _collideCheck = Entity.FindExactAbilityByType<CollideCheck>();
            _playerRb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            if (_collideCheck.IsTouchingGround) PerformWalk();
        }

        public void PerformWalk()
        {
            walkParticle.Play();
        }

        public void UndoPerform()
        {
            walkParticle.Stop();
        }

    }
}