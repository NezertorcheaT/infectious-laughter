using System;
using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Editor.EditorStoryNodes
{
    public class NodeView : Node
    {
        public StoryTree.NodeForList Node;
        public IStateTree<StoryTree.Node> Tree;
        public Port Input;
        public Port Output;
        public event Action<NodeView> OnStateSelected;
        public event Action<NodeView> OnStateUnselected;

        public NodeView(StoryTree.NodeForList node, IStateTree<StoryTree.Node> tree)
        {
            Node = node;
            Tree = tree;
            title = node.visualName;
            viewDataKey = node.id;

            style.left = node.visualPosition.x;
            style.top = node.visualPosition.y;

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
                Port.Capacity.Single,
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

            (Tree as IGlobalParameterNodeStateTree<State, Tuple<Vector2, Color, string, SceneAsset>>)?.TrySetParameters(
                Node.id,
                new Tuple<Vector2, Color, string, SceneAsset>(
                    new Vector2(newPos.xMin, newPos.yMin),
                    Node.visualColor,
                    Node.visualName,
                    Node.scene
                )
            );
        }
    }
}