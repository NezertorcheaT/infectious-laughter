using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Wait State", menuName = "States/Wait State", order = 0)]
    public class WaitState : State, IOneExitState, IEditableState
    {
        public override string Name => "Wait";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous, IEditableState.Properties properties)
        {
            var edit = properties as Edit;

            await Task.Delay((int) (edit.time * 1000f));

            return edit.next;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(Edit);

        [Serializable]
        private class Edit : IEditableState.Properties
        {
            public float time;
            public int next;

            public override T Get<T>(string name) => GetType().GetField(name).GetValue(this) is T
                ? (T) GetType().GetField(name).GetValue(this)
                : default;

            public override void Set<T>(string name, T value) => GetType().GetField(name).SetValue(this, value);
        }
    }
}