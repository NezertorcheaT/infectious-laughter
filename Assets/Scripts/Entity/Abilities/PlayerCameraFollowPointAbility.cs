using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Camera Follow Point Ability")]
    [RequireComponent(typeof(EntityMovementHorizontalMove))]
    [RequireComponent(typeof(PlayerJumpAbility))]
    public class PlayerCameraFollowPointAbility : Ability
    {
        [SerializeField] private Transform followPoint;

        [SerializeField] private float maxRange;
        [SerializeField] private float speed;

        [SerializeField] private EntityMovementHorizontalMove movementAbility;
        [SerializeField] private PlayerJumpAbility jumpAbility;

        private bool _locked = true;

        private void Start()
        {
            movementAbility ??= Entity.FindAbilityByType<EntityMovementHorizontalMove>();
            jumpAbility ??= Entity.FindAbilityByType<PlayerJumpAbility>();
        }

        public void MovePoint(float turn)
        {
            if (_locked) return;
            if (gameObject.transform.position.x - followPoint.position.x < maxRange && turn < 0)
                followPoint.Translate(-speed * Time.deltaTime, 0, 0);

            if (gameObject.transform.position.x - followPoint.position.x > -maxRange && turn > 0)
                followPoint.Translate(speed * Time.deltaTime, 0, 0);
        }

        public void ChangeLock()
        {
            _locked = !_locked;
            followPoint.position = gameObject.transform.position;

            jumpAbility.enabled = _locked;
            movementAbility.enabled = _locked;
        }
    }
}