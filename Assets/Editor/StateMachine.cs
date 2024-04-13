using System.Collections.Generic;
using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class StateMachine : EditorWindow
    {
        private DropdownField _treesDropdown;
        private DropdownField _statesDropdown;
        private Button _stateAddButton;
        private StateTree[] _trees;
        private string[] _treesPaths;
        private State[] _states;
        private string[] _statesPaths;
        private int _selectedTreeStateID;
        private int _currentTreeID;
        private int _selectedStateID;
        private State SelectedState => _states[_selectedStateID];
        private State SelectedTreeState => CurrentTree.GetState(_selectedTreeStateID);
        private StateTree CurrentTree => _trees[_currentTreeID];
        private VisualElement _root;
        private VisualElement _nodes;
        private List<NodeElement> _nodeElements;
        private StateTreeView _stateTreeView;
        private InspectorView _inspectorView;

        [MenuItem("Window/State Machine Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StateMachine>();
            wnd.titleContent = new GUIContent("State Machine");
        }

        public void CreateGUI()
        {
            _root = rootVisualElement;
            //FindTrees();
            //FindStates();

            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/StateMachine.uxml");

            ui.CloneTree(rootVisualElement);
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            rootVisualElement.styleSheets.Add(styleSheet);

            _stateTreeView = rootVisualElement.Q<StateTreeView>();
            _inspectorView = rootVisualElement.Q<InspectorView>();

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is IStateTree tree)
            {
                _stateTreeView.PopulateTree(tree);
            }
        }

        private void FindTrees()
        {
            _treesPaths = AssetDatabase.FindAssets("t:StateTree");
            _treesPaths = _treesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _trees = _treesPaths.Select(AssetDatabase.LoadAssetAtPath<StateTree>).ToArray();

            if (_currentTreeID >= _treesPaths.Length) _currentTreeID = 0;
        }

        private void FindStates()
        {
            _statesPaths = AssetDatabase.FindAssets("t:State");
            _statesPaths = _statesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _states = _statesPaths.Select(AssetDatabase.LoadAssetAtPath<State>).ToArray();
        }

    }
}