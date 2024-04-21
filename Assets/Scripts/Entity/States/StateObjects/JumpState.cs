using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Jump State", menuName = "States/Jump State", order = 0)]
    public class JumpState : State,IOneExitState
    {
        public override string Name => "Jump";

        public override async Task<int> Activate(Entity entity, State previous, EditableStateProperties properties)
        {
            var nextId = 0;
            var ability = entity.FindAbilityByType<EntityMovementJump>();

            if (!ability) return nextId;

            ability.Jump();
            await Task.Delay((int) (ability.JumpTime * 1000f));

            return nextId;
        }
    }
}