using System;
using Entity.States;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
{
    public class StateNodeView : Node
    {
        public StateTree.StateForList State;
        public IStateTree Tree;
        public Port Input;
        public Port Output;
        public event Action<StateNodeView> OnStateSelected;
        public event Action<StateNodeView> OnStateUnselected;

        public StateNodeView(StateTree.StateForList state, IStateTree tree)
        {
            State = state;
            Tree = tree;
            title = state.state.Name;
            viewDataKey = state.id;

            style.left = state.position.x;
            style.top = state.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (Input is not null)
            {
                Input.portName = string.Empty;
                inputContainer.Add(Input);
            }
        }

        private void CreateOutputPorts()
        {
            Output = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                State.state is IOneExitState ? Port.Capacity.Single : Port.Capacity.Multi,
                typeof(bool)
            );

            if (Output is not null)
            {
                Output.portName = string.Empty;
                outputContainer.Add(Output);
            }
        }

        public override void OnSelected()
        {
            OnStateSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            OnStateUnselected?.Invoke(this);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            (Tree as IPositionableStateTree)?.TrySetPosition(State.id, new Vector2(newPos.xMin, newPos.yMin));
        }
    }
}