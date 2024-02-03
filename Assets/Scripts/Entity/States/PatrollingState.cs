using System.Collections.Generic;
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
        protected override string Name => "PatrollingState";

        protected override int Id { get; set; }

        protected override List<IState> Nexts { get; set; }

        protected override void Connect(IState state)
        {
            Nexts.Add(state);
        }

        protected override void Disconnect(IState state)
        {
            Nexts.Remove(state);
        }

        protected override async Task<IState> Activate(Entity entity, IState previous)
        {
            var coll = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            if (!moveAbility) return Nexts[0];
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
            return Nexts[0];
        }
    }
}