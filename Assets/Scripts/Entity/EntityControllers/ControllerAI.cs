using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.States;
using UnityEngine;

namespace Entity.EntityControllers
{
    [RequireComponent(typeof(Collider2D))]
    public class ControllerAI : Controller
    {
        [SerializeField] private StateTree stateTree;
        private StateTree _stateTree => stateTree;
        private bool _stateCycleDestroy;

        public override void Initialize()
        {
            base.Initialize();
            OnInitializationComplete += OnEnable;
            OnEnable();
            StateCycle();
        }

        private async void StateCycle()
        {
            State prew;
            var state = _stateTree.First();

            while (true)
            {
                await Task.Yield();

                if (_stateCycleDestroy) return;
                if (!state) return;
                if (!IsInitialized || !isActiveAndEnabled)
                {
                    await Task.Delay(500);
                    continue;
                }

                prew = state;
                var t = await state.Activate(Entity, prew);

                if (_stateTree.GetNextsTo(state.Id).Length == 0) return;

                state = _stateTree.GetNextsTo(state.Id)[t];
            }
        }

        private void OnEnable()
        {
            if (!IsInitialized) return;
            OnInitializationComplete -= OnEnable;
        }


        private void OnDestroy()
        {
            _stateCycleDestroy = true;
        }
    }
}