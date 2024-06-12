using System.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Downing Ability")]
    public class EntityMovementDowning : Ability
    {
        [SerializeField] private float speed;
        [Space(10f)][SerializeField] private float downingTime;

        private Rigidbody2D _rb;
        private CollideCheck _collideCheck;

        private bool getToWall;
        private bool isDowning;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collideCheck = Entity.FindAbilityByType<CollideCheck>();
        }

        public void WallDowning(float playerInput)
        {
            float downingActive = 0;
            float velX = _rb.velocity.x;
            if (!Available() || !(_collideCheck.RightTrajectory(playerInput) != 0 || getToWall) || _collideCheck.IsTouchingGround) return;

            if (!getToWall)
            {
                getToWall = true;
                isDowning = false;
                WaitSeconds(downingTime);
            }

            if (isDowning) downingActive = -speed;
            if (_collideCheck.RightTrajectory(playerInput) != 0) velX = 0;
            _rb.velocity = new Vector2(velX, downingActive);

            if (!_collideCheck.TestOnWall()) getToWall = false;
        }

        private async void WaitSeconds(float seconds)
        {
            await Task.Delay((int)(seconds * 1000f));
            isDowning = true;
        }
    }
}