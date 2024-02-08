using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Patrolling State", menuName = "States/Patrolling State", order = 0)]
    public class PatrollingState : State
    {
        [SerializeField] private float rayDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        private bool _direction;
        public override string Name => "PatrollingState";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var nextId = 0;
            var coll = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            
            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
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

            return nextId;
        }
    }
}