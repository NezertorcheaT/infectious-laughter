using System;
using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Patrolling State", menuName = "States/Patrolling State", order = 0)]
    public class PatrollingState : State, IEditableState
    {
        public override string Name => "Patrolling";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous, IEditableState.Properties properties)
        {
            var edit = properties as Edit;
            var nextId = edit.next;
            var coll = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            var direction = edit.initialDirection;

            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
                await Task.Yield();

                var ray = new Ray(
                    entity.transform.position + (Vector3) coll.offset +
                    coll.bounds.size.Multiply(new Vector3(direction ? 1 : -1, -1f / 2f, 1)),
                    Vector3.down);
                if (!Physics2D.Raycast(ray.origin, ray.direction, edit.rayDistance, edit.groundLayer))
                    direction = !direction;
                Debug.DrawRay(ray.origin, ray.direction * edit.rayDistance);

                moveAbility.Move(direction ? 1 : -1);
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(Edit);

        [Serializable]
        private class Edit : IEditableState.Properties
        {
            public float rayDistance;
            public LayerMask groundLayer;
            public bool initialDirection;
            public int next;

            public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
                ? (T) GetType().GetField(name).GetValue(this)
                : default;

            public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
        }
    }
}