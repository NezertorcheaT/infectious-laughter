using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static Levels.Generation.LevelGeneration;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "FollowPlayer State", menuName = "AI Nodes/States/FollowPlayer State", order = 0)]
    public class FollowPlayerState : State, IEditableState, IRelationfullState
    {
        [StateEdit] private FollowPlayerStateEdit properties; //
        public override string Name => "FollowPlayer";
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

        Type IEditableState.GetTypeOfEdit() => typeof(FollowPlayerStateEdit); //
    }
}
