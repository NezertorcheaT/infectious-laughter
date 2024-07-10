using System;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        public string CurrentStateID { get; private set; }
        public event Action<State> OnStateActivating;

        private FieldInfo GetStateEditField(Type type, Type editType)
        {
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType.AssemblyQualifiedName != editType.AssemblyQualifiedName) continue;
                foreach (var attributeData in field.CustomAttributes)
                {
                    if (attributeData.AttributeType.AssemblyQualifiedName ==
                        typeof(StateEditAttribute).AssemblyQualifiedName)
                        return field;
                }
            }

            return null;
        }

        private async void StateCycle()
        {
            State previous;
            CurrentState = _stateTree.First();
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
                    _stateTree is IStateTreeWithEdits stateTreeWithEdits
                )
                {
                    var edit = stateTreeWithEdits.GetEdit(CurrentStateID);
                    GetStateEditField
                    (
                        CurrentState.GetType(),
                        editableState.GetTypeOfEdit()
                    )?.SetValue(CurrentState, edit);
                }

                var t = await CurrentState.Activate(
                    Entity,
                    previous
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