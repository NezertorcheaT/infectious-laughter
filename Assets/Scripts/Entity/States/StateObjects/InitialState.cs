using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "AI Nodes/States/Initial State", order = 0)]
    public class InitialState : State, IOneExitState
    {
        public override string Name => "Root";

        public override async Task<int> Activate(Entity entity, State previous) => 0;
    }
}