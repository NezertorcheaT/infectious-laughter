using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Initial State", order = 0)]
    public class InitialState : State
    {
        protected override string Name => "InitialState";

        protected override int Id { get; set; }
        
        protected override async Task<int> Activate(Entity entity, IState previous)
        {
            await Task.Yield();
            return 0;
        }
    }
}