using Entity.States;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
{
    public class StateNodeView : Node
    {
        public State State;

        public StateNodeView(State state)
        {
            State = state;
            title = state.Name;
        }
    }
}
