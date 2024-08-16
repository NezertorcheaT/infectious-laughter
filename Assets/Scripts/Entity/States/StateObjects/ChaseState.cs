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
            var playerInSight = false;
            var lastSeenPosition = Vector3.zero;

            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
                await Task.Yield();

                hostileDetector.direction = direction;
                var (playerEntity, hostileLastSeenPosition) = hostileDetector.Hostile;
                lastSeenPosition = hostileLastSeenPosition ?? lastSeenPosition;

                // ���� ����� � ���� ������
                if (playerEntity)
                {
                    direction = lastSeenPosition.x < entity.transform.position.x;
                    moveAbility.Move(direction ? 1 : -1);
                }
                else
                {
                    // ���� ����� �� � ���� ������, ��������� � ��������� ������� �����
                    if (Vector3.Distance(entity.transform.position, lastSeenPosition) > 0.1f)
                    {
                        if (Math.Abs(entity.transform.position.x - lastSeenPosition.x) < 0.1f)
                        {
                            moveAbility.Move(0);
                            break;
                        }

                        direction = lastSeenPosition.x > entity.transform.position.x;
                        moveAbility.Move(direction ? 1 : -1);
                    }
                    else
                    {
                        // ��������� � ��������������, ���� ����� �� ������
                        break;
                    }
                }
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(PatrollingStateEdit);
    }
}