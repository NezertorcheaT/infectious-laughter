using System;
using System.IO;
using System.Threading.Tasks;
using Entity.EntityMovement;
using Entity.States;
using UnityEngine;

namespace Entity.EntityControllers
{
    [RequireComponent(typeof(Collider2D))]
    public class ControllerAI : Controller
    {
        [SerializeField] private StateTree stateTree;
        private IStateTree _stateTree => stateTree;
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
            var state = _stateTree.First();
            await state.Activate(Entity, null);
            for (;;)
            {
                if (_stateCycleDestroy) return;
                if (!IsInitialized || !isActiveAndEnabled)
                {
                    await Task.Delay(500);
                    continue;
                }

                if (state.Next == null) break;
                await state.Next.Activate(Entity, state);
                state = state.Next;
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