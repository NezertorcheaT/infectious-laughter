using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static Levels.Generation.LevelGeneration;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "FlyPatrolling State", menuName = "AI Nodes/States/FlyPatrolling State", order = 0)]
    public class FlyPatrollingState : State, IEditableState, IRelationfullState
    {
        [StateEdit] private FlyPatrollingStateEdit properties;
        public override string Name => "FlyPatrolling";
        public HostileDetection HostileDetection { get; set; }

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var hostileDetector = entity.GetComponent<DetectionOverview>();
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var flyMoveAbility = entity.FindAbilityByType<Flying>();
            var horizontalmoveAbility = entity.FindAbilityByType<HorizontalMovement>();
            var direction = edit.initialDirection;
            var totalDistance = 0f;

            for (; ;)
            {
                if (!entity) break;
                await Task.Yield();

                hostileDetector.direction = direction;
                if (hostileDetector.Hostile.Item1 is not null) ;
                    return nextId;

                var bounds = collider2d.bounds;
                var horizontalRay = new Ray(
                    bounds.center + new Vector3(direction ? bounds.extents.x : -bounds.extents.x, 0, 0),
                    Vector3.right * (direction ? 1 : -1));

                bool hitWall = Physics2D.Raycast(horizontalRay.origin, horizontalRay.direction, edit.rayDistance, edit.groundLayer);

                if (hitWall)
                    direction = !direction;

                // Полет по синусоиде
                float verticalOffset = Mathf.Sin(Time.time * edit.flightFrequency) * edit.flightAmplitude;

                totalDistance += Mathf.Abs(flyMoveAbility.Speed * Time.deltaTime);
                if (totalDistance >= edit.maxFlightDistance)
                {
                    direction = !direction;                     
                    totalDistance = 0;
                }

                horizontalmoveAbility.Move(direction ? 1 : -1, 1);
                flyMoveAbility.Flight(verticalOffset);
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(FlyPatrollingStateEdit);
    }
}

