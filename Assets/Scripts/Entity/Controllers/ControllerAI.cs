using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using Entity.States;
using UnityEngine;

namespace Entity.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Controllers/AI Controller")]
    public class ControllerAI : Controller
    {
        [SerializeField] private StateTree stateTree;
        [SerializeField] private HostileDetection hostileDetection;
        private IStateTree<State> StateTree => stateTree;
        private bool _stateCycleDestroy;

        public override void Initialize()
        {
            base.Initialize();
            OnInitializationComplete += OnEnable;
            OnEnable();
            StateCycle();
        }

        public State CurrentState { get; private set; }
        public string CurrentStateID { get; private set; }
        public event Action<State> OnStateActivating;

        private async void StateCycle()
        {
            State previous;
            CurrentState = StateTree.First();
            CurrentStateID = "0";

            while (true)
            {
                await Task.Yield();

                if (_stateCycleDestroy) return;
                if (!CurrentState) return;

                await UniTask.WaitUntil(() => IsInitialized && isActiveAndEnabled);

                previous = CurrentState;
                OnStateActivating?.Invoke(CurrentState);
                if (
                    CurrentState is IEditableState editableState &&
                    StateTree is IStateTreeWithEdits stateTreeWithEdits
                )
                {
                    var edit = stateTreeWithEdits.GetEdit(CurrentStateID);
                    StateEditAttribute.GetStateEditField
                    (
                        CurrentState.GetType(),
                        editableState.GetTypeOfEdit()
                    )?.SetValue(CurrentState, edit);
                }

                if (CurrentState is IRelationfullState relationfullState)
                    relationfullState.HostileDetection = hostileDetection;

                var t = await CurrentState.Activate(
                    Entity,
                    previous
                );

                if (!StateTree.IsNextsTo(CurrentStateID)) return;

                CurrentStateID = StateTree.GetNextsTo(CurrentStateID)[t];
                CurrentState = StateTree.GetState(CurrentStateID);
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