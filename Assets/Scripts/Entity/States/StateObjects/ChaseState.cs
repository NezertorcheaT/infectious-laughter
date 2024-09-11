using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Chase State", menuName = "AI Nodes/States/Chase State", order = 0)]
    public class ChaseState : State, IEditableState, IRelationfullState
    {
        [StateEdit] private PatrollingStateEdit properties;

        public override string Name => "Chase";

        public HostileDetection HostileDetection { get; set; }

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var hostileDetector = HostileDetection;
            var nextId = edit.next;
            var moveAbility = entity.FindAbilityByType<HorizontalMovement>();
            var direction = edit.initialDirection;
            var lastSeenPosition = Vector3.zero;

            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
                await Task.Yield();

                hostileDetector.direction = direction;
                var (playerEntity, hostileLastSeenPosition) = hostileDetector.Hostile;
                lastSeenPosition = hostileLastSeenPosition ?? lastSeenPosition;

                // ≈сли игрок в поле зрени€
                if (playerEntity)
                {
                    direction = lastSeenPosition.x > entity.transform.position.x;
                    moveAbility.Move(direction ? 1 : -1);
                }
                else
                {
                    // ≈сли игрок не в поле зрени€, двигатьс€ к последней видимой точке
                    if (Vector3.Distance(entity.transform.position, lastSeenPosition) > 1f)
                    {
                        direction = lastSeenPosition.x > entity.transform.position.x;
                        moveAbility.Move(direction ? 1 : -1);
                        continue;
                    }

                    moveAbility.Move(0);
                    break;
                }
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(PatrollingStateEdit);
    }
}