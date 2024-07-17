using System;
using System.Collections.Generic;
using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using State = Entity.States.State;

namespace Editor.EditorAI
{
    public class StateTreeView : GraphView
    {
        public event Action<StateNodeView> OnStateSelected;
        public event Action<StateNodeView> OnStateUnselected;

        public new class UxmlFactory : UxmlFactory<StateTreeView, GraphView.UxmlTraits>
        {
        }

        private IStateTree<State> _tree;

        public StateTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachine.USS);
            styleSheets.Add(styleSheet);
        }

        private StateNodeView FindStateView(string id)
        {
            return GetNodeByGuid(id) as StateNodeView;
        }

        public void PopulateTree(IStateTree<State> tree)
        {
            _tree = tree;

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
                    position = ((IGlobalParameterNodeStateTree<State, Tuple<Vector2>>) _tree).GetParameters(id).Item1
                });
            }

            foreach (var id in _tree.Nodes.Keys)
            {
                var childrens = _tree.GetNextsTo(id);

                foreach (var childrenID in childrens)
                {
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
                    {
                        _tree.TryRemoveState(nodeView.State.id);
                    }

                    if (element is Edge edge)
                    {
                        var parentView = edge.output.node as StateNodeView;
                        var childView = edge.input.node as StateNodeView;
                        _tree.TryDisconnect(parentView.State.id, childView.State.id);
                    }
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
            var types = TypeCache.GetTypesDerivedFrom<State>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", a => CreateNode(type));
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
                position = ((IGlobalParameterNodeStateTree<State, Tuple<Vector2>>) _tree).GetParameters(newState).Item1
            });
        }

        private void CreateNodeView(StateTree.StateForList state)
        {
            var nodeView = new StateNodeView(state, _tree);
            if (OnStateSelected != null) nodeView.OnStateSelected += OnStateSelected.Invoke;
            if (OnStateUnselected != null) nodeView.OnStateUnselected += OnStateUnselected.Invoke;
            AddElement(nodeView);
        }
    }
}