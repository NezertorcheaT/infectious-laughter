using System;
using System.Collections.Generic;
using System.Linq;
using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.EditorStoryNodes
{
    public class StoryTreeView : GraphView
    {
        public event Action<NodeView> OnStateSelected;
        public event Action<NodeView> OnStateUnselected;

        public new class UxmlFactory : UxmlFactory<StoryTreeView, UxmlTraits>
        {
        }

        private IStateTree<StoryTree.Node> _tree;
        private ITwoPerConnectionStateTree<StoryTree.Node> _connectionTree;
        private IZoomableStateTree<StoryTree.Node> _zoomableTree;
        public bool Populated;
        public IEnumerable<NodeView> NodeViews => nodes.Select(i => i as NodeView);

        public StoryTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StoryInspector.USS);
            styleSheets.Add(styleSheet);

            viewTransformChanged += ViewportChanged;
        }

        private void ViewportChanged(GraphView graphView)
        {
            _zoomableTree.Position = graphView.viewTransform.position;
            _zoomableTree.Rotation = graphView.viewTransform.rotation;
            _zoomableTree.Scale = graphView.viewTransform.scale;
        }

        private NodeView FindStateView(string id) =>
            NodeViews.FirstOrDefault(i => i.Node.id == id) ?? GetNodeByGuid(id) as NodeView;

        public void PopulateTree(IStateTree<StoryTree.Node> tree)
        {
            _tree = tree;
            _connectionTree = tree as ITwoPerConnectionStateTree<StoryTree.Node>;
            _zoomableTree = tree as IZoomableStateTree<StoryTree.Node>;

            if (_zoomableTree != null)
            {
                viewTransform.position = _zoomableTree.Position;
                viewTransform.rotation = _zoomableTree.Rotation;
                viewTransform.scale = _zoomableTree.Scale;
            }

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            foreach (var node in _tree.Nodes.Values)
            {
                var nodeToListed = StoryTree.NodeToListed(node, _tree);
                CreateNodeView(nodeToListed);
            }

            if (!(_tree is ITwoPerConnectionStateTree<StoryTree.Node> tpt)) return;
            foreach (var (id, value) in tpt.Nodes)
            {
                var port1 = tpt.GetPort1(id);
                var port2 = tpt.GetPort2(id);
                var parentView = FindStateView(id);

                if (port1 != string.Empty)
                {
                    var edge1 = parentView.Output1.ConnectTo(FindStateView(port1).Input);
                    AddElement(edge1);
                }

                if (port2 != string.Empty)
                {
                    var edge2 = parentView.Output2.ConnectTo(FindStateView(port2).Input);
                    AddElement(edge2);
                }
            }

            Populated = true;
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
                    if (element is NodeView nodeView)
                        _tree.TryRemoveState(nodeView.Node.id);

                    if (!(element is Edge edge)) continue;
                    var parentView = edge.output.node as NodeView;
                    var childView = edge.input.node as NodeView;

                    if (edge.output.portName == NodeView.Output1Text)
                        _connectionTree.TryDisconnectAPort1(parentView.Node.id, childView.Node.id);
                    if (edge.output.portName == NodeView.Output2Text)
                        _connectionTree.TryDisconnectAPort2(parentView.Node.id, childView.Node.id);
                }
            }

            if (graphViewChange.edgesToCreate is not null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var parentView = edge.output.node as NodeView;
                    var childView = edge.input.node as NodeView;

                    if (edge.output.portName == NodeView.Output1Text)
                        _connectionTree.TryConnectToPort1(parentView.Node.id, childView.Node.id);
                    if (edge.output.portName == NodeView.Output2Text)
                        _connectionTree.TryConnectToPort2(parentView.Node.id, childView.Node.id);
                }
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("New Level", a => CreateNode());
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