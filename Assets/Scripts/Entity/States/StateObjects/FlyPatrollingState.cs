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

        private bool _goToNext;
        private DetectionOverview _hostileDetector;
        private Flying _flyMoveAbility;
        private HorizontalMovement _horizontalmoveAbility;
        private EnemyMemory _enemyMemory;

        public override async Task<int> Activate(Entity entity, State previous)
        {
            Debug.Log("патрулирование");
            var edit = properties;
            _hostileDetector = entity.GetComponent<DetectionOverview>();
            _flyMoveAbility = entity.FindAbilityByType<Flying>();
            _horizontalmoveAbility = entity.FindAbilityByType<HorizontalMovement>();
            _enemyMemory = entity.GetComponent<EnemyMemory>();
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var direction = edit.initialDirection;
            var totalDistance = 0f;

            _goToNext = false;
            _horizontalmoveAbility.Speed = edit.flightSpeed;
            _hostileDetector.enabled = true;
            _enemyMemory.StartPosition = entity.transform.position;

            _hostileDetector.HostileEntitieDetected += StartFindingEnemy;

            for (; ;)
            {
                if (!entity) break;
                await Task.Yield();
                
                if (_goToNext)
                    return nextId;

                var bounds = collider2d.bounds;
                var horizontalRay = new Ray(
                    bounds.center + new Vector3(direction ? bounds.extents.x : -bounds.extents.x, 0, 0),
                    Vector3.right * 5 * (direction ? 1 : -1));
                
                bool hitWall = Physics2D.Raycast(horizontalRay.origin, horizontalRay.direction, edit.rayDistance, edit.groundLayer);

                if (hitWall)
                    direction = !direction;

                // Полет по синусоиде
                float verticalOffset = Mathf.Sin(Time.time * edit.flightFrequency) * edit.flightAmplitude;

                totalDistance += Mathf.Abs(_flyMoveAbility.Speed * Time.deltaTime);
                if (totalDistance >= edit.maxFlightDistance)
                {
                    direction = !direction;                     
                    totalDistance = 0;
                }

                _horizontalmoveAbility.Move(direction ? 1 : -1, 1);
                _flyMoveAbility.Flight(verticalOffset);
             
            }

            return nextId;
        }

        private void StartFindingEnemy(Entity entity)
        {
            _enemyMemory.LastPlayerPosition = _hostileDetector.HostileEntities[0].transform.position;

            _horizontalmoveAbility.Speed = properties.findingEnemySpeed;
            _hostileDetector.HostileEntitieDetected -= StartFindingEnemy;

            if (_hostileDetector.FriendlyEntities.Count > 0) 
                StartFollowEnemy(_hostileDetector.FriendlyEntities[0]);
            else _hostileDetector.FriendlyEntitieDetected += StartFollowEnemy;
        }
        private void StartFollowEnemy(Entity entity)
        {
            _enemyMemory.FollowEnemy = entity;
            _goToNext = true;
            _hostileDetector.enabled = false;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(FlyPatrollingStateEdit);
    }
}

