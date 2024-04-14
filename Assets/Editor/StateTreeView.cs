using System;
using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using State = Entity.States.State;

namespace Editor
{
    public class StateTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<StateTreeView, GraphView.UxmlTraits>
        {
        }

        private IStateTree _tree;

        public StateTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            styleSheets.Add(styleSheet);
        }

        public void PopulateTree(IStateTree tree)
        {
            _tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            foreach (var id in tree.States.Keys)
            {
                CreateNodeView(new StateTree.StateForList
                {
                    id = id, 
                    nexts = tree.GetNextsTo(id).Select(state => state.Id).ToList(), 
                    state = tree.GetState(id),
                    position = ((IPositionableStateTree) tree).GetPosition(id)
                });
            }
        }

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

        void CreateNode(Type type)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{type.Name}").First()));
            Debug.Log(AssetDatabase.LoadAssetAtPath<State>(AssetDatabase.FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath).First()));
            Debug.Log(AssetDatabase.LoadAssetAtPath<State>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{type.Name}").First())));
            Debug.Log(AssetDatabase.LoadAssetAtPath(
                AssetDatabase.FindAssets($"t:{type.Name}").Select(AssetDatabase.GUIDToAssetPath).First(),
                type) as State);

            _tree.AddState(AssetDatabase.LoadAssetAtPath(AssetDatabase.FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath).First(), type) as State);
        }

        void CreateNodeView(StateTree.StateForList state)
        {
            var nodeView = new StateNodeView(state, _tree as IPositionableStateTree);
            AddElement(nodeView);
        }
    }
}