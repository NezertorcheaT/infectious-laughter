using System;
using System.Collections.Generic;
using System.Linq;
using Editor.EditorAI;
using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.EditorStoryNodes
{
    public class StoryTreeView : GraphView
    {
        public event Action<NodeView> OnStateSelected;
        public event Action<NodeView> OnStateUnselected;

        public new class UxmlFactory : UxmlFactory<StoryTreeView, GraphView.UxmlTraits>
        {
        }

        private IStateTree<StoryTree.Node> _tree;

        public StoryTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StoryView.USS);
            styleSheets.Add(styleSheet);
        }

        private NodeView FindStateView(string id)
        {
            return GetNodeByGuid(id) as NodeView;
        }

        public void PopulateTree(IStateTree<StoryTree.Node> tree)
        {
            _tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            foreach (var id in _tree.Nodes.Keys)
            {
                CreateNodeView(StoryTree.NodeToListed(_tree.GetState(id), _tree));
            }

            foreach (var id in _tree.Nodes.Keys)
            {
                var childrens = _tree.GetNextsTo(id);

                foreach (var childrenID in childrens)
                {
                    var children = _tree.GetState(childrenID);
                    var parentView = FindStateView(id);
                    var childView = FindStateView(childrenID);

                    var edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) => ports.ToList()
            .Where(
                endPort =>
                    endPort.direction != startPort.direction &&
                    endPort.node != startPort.node
            ).ToList();

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove is not null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is StateNodeView nodeView)
                        _tree.TryRemoveState(nodeView.State.id);

                    if (!(element is Edge edge)) continue;
                    var parentView = edge.output.node as StateNodeView;
                    var childView = edge.input.node as StateNodeView;
                    _tree.TryDisconnect(parentView.State.id, childView.State.id);
                }
            }

            if (graphViewChange.edgesToCreate is not null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var parentView = edge.output.node as StateNodeView;
                    var childView = edge.input.node as StateNodeView;
                    _tree.TryConnect(parentView.State.id, childView.State.id);
                }
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            evt.menu.AppendAction($"New Level", a => CreateNode());
        }

        private void CreateNode()
        {
            var id = _tree.AddState(new StoryTree.Node());
            CreateNodeView(StoryTree.NodeToListed(_tree.GetState(id), _tree));
        }

        private void CreateNodeView(StoryTree.NodeForList node)
        {
            var nodeView = new NodeView(node, _tree);
            if (OnStateSelected != null) nodeView.OnStateSelected += OnStateSelected.Invoke;
            if (OnStateUnselected != null) nodeView.OnStateUnselected += OnStateUnselected.Invoke;
            AddElement(nodeView);
        }
    }
}