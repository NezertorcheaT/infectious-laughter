using Entity.States;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
{
    public class StateNodeView : Node
    {
        public StateTree.StateForList State;
        public IPositionableStateTree Tree;

        public StateNodeView(StateTree.StateForList state,IPositionableStateTree tree)
        {
            State = state;
            Tree = tree;
            title = state.state.Name;
            viewDataKey = state.id.ToString();
            
            style.left = state.position.x;
            style.top = state.position.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Tree.TrySetPosition(State.id, new Vector2(newPos.xMin, newPos.yMin));
        }
    }
}
