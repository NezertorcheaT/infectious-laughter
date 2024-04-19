using System;
using System.Threading.Tasks;
using Entity.States;
using UnityEngine;

namespace Entity.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Controllers/AI Controller")]
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

        public State CurrentState { get; private set; }
        public event Action<State> OnStateActivating;

        private async void StateCycle()
        {
            State prew;
            CurrentState = _stateTree.First();

            while (true)
            {
                await Task.Yield();

                if (_stateCycleDestroy) return;
                if (!CurrentState) return;
                if (!IsInitialized || !isActiveAndEnabled)
                {
                    await Task.Delay(500);
                    continue;
                }

                prew = CurrentState;
                OnStateActivating?.Invoke(CurrentState);

                var t = await CurrentState.Activate(
                    Entity,
                    prew,
                    CurrentState is IEditableState &&
                    _stateTree is IStateTreeWithEdits stateTreeWithEdits
                        ? stateTreeWithEdits.GetEdit(CurrentState.Id)
                        : null
                );

                if (_stateTree.GetNextsTo(CurrentState.Id).Length == 0) return;

                CurrentState = _stateTree.GetNextsTo(CurrentState.Id)[t];
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