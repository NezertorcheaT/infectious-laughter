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
        private IStateTree _stateTree => stateTree;
        private bool _stateCycleDestroy;

        public override void Initialize()
        {
            base.Initialize();
            OnInitializationComplete += OnEnable;
            OnEnable();
            StateCycle();
        }

        public State CurrentState { get; private set; }
        public int CurrentStateID { get; private set; }
        public event Action<State> OnStateActivating;

        private async void StateCycle()
        {
            State prew;
            CurrentState = _stateTree.First();
            CurrentStateID = 0;

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
                        ? stateTreeWithEdits.GetEdit(CurrentStateID)
                        : null
                );

                if (!_stateTree.IsNextsTo(CurrentStateID)) return;

                CurrentStateID = _stateTree.GetNextsTo(CurrentStateID)[t];
                CurrentState = _stateTree.GetState(CurrentStateID);
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