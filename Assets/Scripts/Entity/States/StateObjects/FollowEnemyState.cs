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

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var hostileDetector = HostileDetection;
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<HorizontalMovement>();
            var direction = edit.initialDirection;

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(FollowEnemyStateEdit); 
    }
}
