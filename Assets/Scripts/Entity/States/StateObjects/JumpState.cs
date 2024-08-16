using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Jump State", menuName = "AI Nodes/States/Jump State", order = 0)]
    public class JumpState : State, IOneExitState
    {
        public override string Name => "Jump";

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var nextId = 0;
            var ability = entity.FindAvailableAbilityByInterface<IJumpableAbility>();

            if (ability is null) return nextId;

            ability.Perform();
            await Task.Delay((int) (ability.JumpTime * 1000f));

            return nextId;
        }
    }
}