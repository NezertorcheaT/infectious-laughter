using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Downing Ability")]
    public class EntityMovementDowning : Ability
    {
        [SerializeField] private float speed;

        private Rigidbody2D _rb;
        private CollideCheck _collideCheck;

        private bool getToWallOnce;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
        }

        public void WallDowning(float playerInput)
        {
            if (!Available()) return;
            if (!(_collideCheck.RightTrajectory(playerInput) != 0 || getToWallOnce)) return;
            getToWallOnce = true;
            _rb.velocity = new Vector2(0, -speed);
            if (!_collideCheck.TestOnWall()) getToWallOnce = false;
        }
    }
}