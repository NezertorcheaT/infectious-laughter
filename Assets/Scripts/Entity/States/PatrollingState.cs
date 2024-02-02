using System.Threading.Tasks;
using Entity.EntityMovement;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Patrolling State", order = 0)]
    public class PatrollingState : State
    {
        [SerializeField] private float rayDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        private bool _direction;
        protected override string Name => "InitialState";

        protected override int Id { get; set; }

        protected override IState Next { get; set; }

        protected override async Task Activate(Entity entity, IState previous)
        {
            var coll = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            if (!moveAbility) return;
            for (;;)
            {
                await Task.Yield();

                var ray = new Ray(
                    entity.transform.position + (Vector3) coll.offset +
                    coll.bounds.size.Multiply(new Vector3(_direction ? 1 : -1, -1f / 2f, 1)),
                    Vector3.down);
                if (!Physics2D.Raycast(ray.origin, ray.direction, rayDistance, groundLayer))
                    _direction = !_direction;
                Debug.DrawRay(ray.origin, ray.direction * rayDistance);

                moveAbility.Move(_direction ? 1 : -1);
            }
        }
    }
}