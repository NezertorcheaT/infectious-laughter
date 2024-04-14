using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Wait State", menuName = "States/Wait State", order = 0)]
    public class WaitState : State,IOneExitState
    {
        [SerializeField, Min(0)] private float time = 2f;
        public override string Name => "Wait";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var nextId = 0;

            await Task.Delay((int) (time * 1000f));

            return nextId;
        }
    }
}