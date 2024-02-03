using System;
using System.Collections.Generic;
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
            var states = new List<IState> {_stateTree.First()};
            await states[0].Activate(Entity, null);
            for (;;)
            {
                if (_stateCycleDestroy) return;
                if (states.Count == 0) return;
                if (!IsInitialized || !isActiveAndEnabled)
                {
                    await Task.Delay(500);
                    continue;
                }

                var newStates = new List<IState>(0);
                foreach (var state in states)
                {
                    if (state.Nexts.Count == 0) break;
                    foreach (var next in state.Nexts)
                    {
                        await next.Activate(Entity, state);
                        newStates.AddRange(next.Nexts);
                    }
                }

                states = newStates;
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