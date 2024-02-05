using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Initial State", order = 0)]
    public class InitialState : State
    {
        public override string Name => "InitialState";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous) => 0;
    }
}