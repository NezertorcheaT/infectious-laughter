using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomHelper;
using Entity.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.EditorInspectorAI
{
    public class AiTreeView : GraphView
    {
        public event Action<StateView> OnStateSelected;
        public event Action<StateView> OnStateUnselected;

        public new class UxmlFactory : UxmlFactory<AiTreeView, UxmlTraits>
        {
        }

        private IStateTree<State> _tree;
        private IZoomableStateTree<State> _zoomableTree;
        public bool Populated;
        public IEnumerable<StateView> NodeViews => nodes.Select(i => i as StateView);

        public AiTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AiInspector.USS);
            styleSheets.Add(styleSheet);

            viewTransformChanged += ViewportChanged;
        }

        private void ViewportChanged(GraphView graphView)
        {
            _zoomableTree.Position = graphView.viewTransform.position;
            _zoomableTree.Rotation = graphView.viewTransform.rotation;
            _zoomableTree.Scale = graphView.viewTransform.scale;
        }

        private StateView FindStateView(string id) =>
            NodeViews.FirstOrDefault(i => i.State.id == id) ?? GetNodeByGuid(id) as StateView;

        public void PopulateTree(IStateTree<State> tree)
        {
            _tree = tree;
            _zoomableTree = tree as IZoomableStateTree<State>;

            if (_zoomableTree != null)
            {
                viewTransform.position = _zoomableTree.Position;
                viewTransform.rotation = _zoomableTree.Rotation;
                viewTransform.scale = _zoomableTree.Scale;
            }

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            foreach (var id in _tree.Nodes.Keys)
            {
                CreateNodeView(new StateTree.StateForList
                {
                    id = id,
                    nexts = _tree.GetNextsTo(id).ToList(),
                    state = _tree.GetState(id),
                    position = ((IGlobalParameterNodeStateTree<State, Tuple<Vector2>>)_tree).GetParameters(id).Item1
                });
            }

            foreach (var id in _tree.Nodes.Keys)
            {
                var childrens = _tree.GetNextsTo(id);

                foreach (var childrenID in childrens)
                {
                    var parentView = FindStateView(id);
                    var childView = FindStateView(childrenID);

                    var edge = parentView.Outputs.FirstOrDefault(c => c.tooltip == childrenID)
                        ?.ConnectTo(childView.Input);
                    if (edge is null)
                    {
                        Debug.Log(childrenID);
                        Debug.Log(parentView.Outputs.Select(a => a.tooltip).ToLog());
                        continue;
                    }

                    AddElement(edge);
                }
            }

            Populated = true;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) => ports
            .Where(
                endPort =>
                    endPort.direction != startPort.direction &&
                    endPort.node != startPort.node
            )
            .ToList();

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove is not null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is StateView nodeView)
                    {
                        nodeView.DeinitializeEditor();
                        _tree.TryRemoveState(nodeView.State.id);
                        continue;
                    }

                    if (element is not Edge edge) continue;

                    var parentView = edge.output.node as StateView;
                    var childView = edge.input.node as StateView;

                    if (!_tree.TryDisconnect(parentView.State.id, childView.State.id)) continue;
                    if (graphViewChange.edgesToCreate.Select(e => e.output.tooltip).Contains(edge.output.tooltip))
                        continue;

                    var port = parentView.outputContainer.Children()
                        .First(i => (i as Port)?.portName == childView.State.id) as Port;
                    parentView.outputContainer.Remove(port);
                    parentView.Outputs.Remove(port);
                }
            }

            if (graphViewChange.edgesToCreate is not null)
            {
                var l = new List<Edge>();
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var parentView = edge.output.node as StateView;
                    var childView = edge.input.node as StateView;

                    if (!_tree.TryConnect(parentView.State.id, childView.State.id))
                    {
                        l.Add(edge);
                        continue;
                    }

                    if (edge.output.portName == StateView.NewOutputText)
                    {
                        var port = edge.output.node.InstantiatePort(
                            Orientation.Horizontal,
                            Direction.Output,
                            Port.Capacity.Single,
                            typeof(bool)
                        );
                        port.portName = childView.State.id;
                        parentView.outputContainer.Insert(parentView.outputContainer.childCount - 1, port);
                        parentView.Outputs.Add(port);
                        edge.output = port;
                    }
                }

                foreach (var e in l)
                {
                    graphViewChange.edgesToCreate.Remove(e);
                }
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            var types = TypeCache.GetTypesDerivedFrom<State>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type));
            }
        }

        private void CreateNode(Type type)
        {
            if (AssetDatabase.FindAssets($"t:{type.Name}").Length == 0)
                _tree.CreateMissingStateObject(type);

            var newState = _tree.AddState(AssetDatabase.LoadAssetAtPath(AssetDatabase.FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath).First(), type) as State);
            CreateNodeView(new StateTree.StateForList
            {
                id = newState,
                nexts = _tree.GetNextsTo(newState).ToList(),
                state = _tree.GetState(newState),
                position = ((IGlobalParameterNodeStateTree<State, Tuple<Vector2>>)_tree).GetParameters(newState).Item1
            });
        }

        private void CreateNodeView(StateTree.StateForList state)
        {
            var nodeView = new StateView(state, _tree);
            if (OnStateSelected != null) nodeView.OnStateSelected += OnStateSelected.Invoke;
            if (OnStateUnselected != null) nodeView.OnStateUnselected += OnStateUnselected.Invoke;
            AddElement(nodeView);
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static string ToLog<T>(this IEnumerable<T> collection) => collection.ToArray().ToLog();

        public static string ToLog<T>(this ICollection<T> collection)
        {
            if (collection.Count == 0)
                return $"{collection.GetType().Name}{{ }}";
            var sb = new StringBuilder();
            sb.Append($"{collection.GetType().Name}{{ ");
            foreach (var o in collection.SkipLast(1))
            {
                sb.Append(o);
                sb.Append(", ");
            }

            sb.Append(collection.Last());
            sb.Append(" }");
            return sb.ToString();
        }
    }
}