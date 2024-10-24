using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static Levels.Generation.LevelGeneration;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "FollowEnemy State", menuName = "AI Nodes/States/FollowEnemy State", order = 0)]
    public class FollowEnemyState : State, IEditableState, IRelationfullState
    {
        [StateEdit] private FollowEnemyStateEdit properties; 
        public override string Name => "FollowEnemy";
        public HostileDetection HostileDetection { get; set; }
        private Flying _flyMoveAbility;
        private HorizontalMovement _horizontalmoveAbility;
        private EnemyMemory _enemyMemory;
        float stopDistance = 1.0f;

        public override async Task<int> Activate(Entity entity, State previous)
        {
            Debug.Log("преследывание врага");
            var edit = properties;
            var hostileDetector = HostileDetection;
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            _horizontalmoveAbility = entity.FindAbilityByType<HorizontalMovement>();
            _flyMoveAbility = entity.FindAbilityByType<Flying>();
            _enemyMemory = entity.GetComponent<EnemyMemory>();
            var direction = edit.initialDirection;
            var followEntity = _enemyMemory.FollowEnemy;

            for (; ; )
            {
                if (!entity || !followEntity) break;

                var currentPosition = entity.transform.position;
                var targetPosition = followEntity.transform.position;

                float horizontalDiff = targetPosition.x - currentPosition.x;
                float verticalDiff = targetPosition.y - currentPosition.y;

                if (Mathf.Abs(horizontalDiff) < stopDistance && Mathf.Abs(verticalDiff) < stopDistance)
                {
                    followEntity.transform.SetParent(_enemyMemory.Entity.transform);
                    break;
                }

                bool moveRight = horizontalDiff > 0;
                _horizontalmoveAbility.Move(moveRight ? 1 : -1, 1);

                float verticalOffset = Mathf.Sign(verticalDiff);
                _flyMoveAbility.Flight(verticalOffset);

                await Task.Yield();
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(FollowEnemyStateEdit); 
    }
}
