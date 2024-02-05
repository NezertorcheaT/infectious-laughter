using System.IO;
using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Editor
{
    public class StateMachine : EditorWindow
    {
        private Label _label;
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

        [MenuItem("Window/UI Toolkit/State Machine Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StateMachine>();
            wnd.titleContent = new GUIContent("State Machine");
        }

        private void CreateGUI()
        {
            _root = rootVisualElement;
            FindTrees();
            FindStates();

            VisualElement ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/StateMachine.uxml")
                .CloneTree();

            _label = new Label();
            _nodes = ui.Q<VisualElement>("Nodes");
            _treesDropdown = ui.Q<DropdownField>("Trees");
            _treesDropdown.choices = _treesPaths.Select(Path.GetFileNameWithoutExtension).ToList();
            _treesDropdown.index = 0;
            _treesDropdown.RegisterValueChangedCallback(evt => OnPickTree());

            _statesDropdown = ui.Q<DropdownField>("States");
            _statesDropdown.choices = _statesPaths.Select(Path.GetFileNameWithoutExtension).ToList();
            _statesDropdown.index = 0;
            _statesDropdown.RegisterValueChangedCallback(evt => OnPickState());

            _stateAddButton = ui.Q<Button>("AddState");
            _stateAddButton.clicked += AddState;

            _nodes.Add(_label);
            _root.Add(ui);
        }

        private void OnInspectorUpdate()
        {
            if (!hasFocus) return;
            FindTrees();
            FindStates();
            UpdateDropdown();
            if (_trees.Length == 0)
            {
                _label.SetEnabled(false);
                _treesDropdown.SetEnabled(false);
                _statesDropdown.SetEnabled(false);
            }

            _label.text = "";
            _label.text += $"{CurrentTree.States.Count}";
        }

        private void AddState()
        {
            CurrentTree.AddState(SelectedState);
            CurrentTree.TryConnect(SelectedTreeState.Id, SelectedState.Id);

            FindTrees();
            FindStates();
        }

        private void UpdateDropdown()
        {
            _treesDropdown.choices = _treesPaths.Select(Path.GetFileNameWithoutExtension).ToList();
            _statesDropdown.choices = _statesPaths.Select(Path.GetFileNameWithoutExtension).ToList();
        }

        private void OnPickTree()
        {
            _currentTreeID = _treesDropdown.index;
        }

        private void OnPickState()
        {
            _selectedStateID = _statesDropdown.index;
        }

        private void FindTrees()
        {
            _treesPaths = AssetDatabase.FindAssets("t:StateTree");
            _treesPaths = _treesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _trees = _treesPaths.Select(AssetDatabase.LoadAssetAtPath<StateTree>).ToArray();
        }

        private void FindStates()
        {
            _statesPaths = AssetDatabase.FindAssets("t:State");
            _statesPaths = _statesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _states = _statesPaths.Select(AssetDatabase.LoadAssetAtPath<State>).ToArray();
        }

        private void Update()
        {
            if (!hasFocus) return;
            _label.transform.position = Mouse.current.position.ReadValue() - (Vector2) _nodes.transform.position -
                                        position.position;
        }
    }
}