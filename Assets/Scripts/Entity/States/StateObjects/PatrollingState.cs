using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Patrolling State", menuName = "AI Nodes/States/Patrolling State", order = 0)]
    public class PatrollingState : State, IEditableState, IRelationfullState
    {
        [StateEdit] private PatrollingStateEdit properties;
        public override string Name => "Patrolling";
        public HostileDetection HostileDetection { get; set; }

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var hostileDetector = HostileDetection;
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<HorizontalMovement>();
            var direction = edit.initialDirection;

            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
                await Task.Yield();

                // Переключиться в состояние преследования
                hostileDetector.direction = direction;
                if (hostileDetector.Hostile.Item1 is not null)
                    return nextId;

                // Патрулирование
                var bounds = collider2d.bounds;

                var rayPit = new Ray(
                    bounds.center +
                    bounds.size.Multiply(new Vector3(direction ? 1 : -1, -1f / 2f, 1)),
                    Vector3.down);
                var rayWall = new Ray(
                    bounds.center + (collider2d.bounds.extents +
                                     new Vector3(edit.rayDistance, 0, 0)).Multiply(direction ? 1 : -1, 1, 0),
                    Vector3.down);
                if (
                    !Physics2D.Raycast(rayPit.origin, rayPit.direction, edit.rayDistance, edit.groundLayer) ||
                    Physics2D.Raycast(rayWall.origin, rayWall.direction, bounds.size.y, edit.groundLayer)
                )
                    direction = !direction;
                //Debug.DrawRay(rayPit.origin, rayPit.direction * edit.rayDistance);
                //Debug.DrawRay(rayWall.origin, rayWall.direction, Color.blue, bounds.size.y);

                moveAbility.Move(direction ? 1 : -1, 0);
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(PatrollingStateEdit);
    }
}