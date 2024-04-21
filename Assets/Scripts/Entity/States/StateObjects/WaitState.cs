using System;
using System.Threading.Tasks;
using Entity.States.StateObjects.Edits;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Wait State", menuName = "States/Wait State", order = 0)]
    public class WaitState : State, IOneExitState, IEditableState
    {
        public override string Name => "Wait";

        public override async Task<int> Activate(Entity entity, State previous, EditableStateProperties properties)
        {
            var edit = properties as WaitStateEdit;

            await Task.Delay((int) (edit.time * 1000f));

            return edit.next;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(WaitStateEdit);
    }
}