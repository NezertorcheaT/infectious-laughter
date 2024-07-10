using System;
using System.Threading.Tasks;
using Entity.States.StateObjects.Edits;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Wait State", menuName = "AI Nodes/States/Wait State", order = 0)]
    public class WaitState : State, IOneExitState, IEditableState
    {
        [StateEdit] private WaitStateEdit properties;
        public override string Name => "Wait";

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;

            await Task.Delay((int) (edit.time * 1000f));

            return 0;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(WaitStateEdit);
    }
}