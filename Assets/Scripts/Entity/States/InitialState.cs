using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Initial State", order = 0)]
    public class InitialState : State
    {
        protected override string Name => "InitialState";

        protected override int Id { get; set; }

        protected override List<IState> Nexts { get; set; }

        protected override void Connect(IState state)
        {
            Nexts ??= new List<IState>(0);
            Nexts.Add(state);
        }

        protected override void Disconnect(IState state)
        {
            Nexts ??= new List<IState>(0);
            Nexts.Remove(state);
        }

        protected override async Task<IState> Activate(Entity entity, IState previous)
        {
            await Task.Yield();
            return Nexts[0];
        }
    }
}