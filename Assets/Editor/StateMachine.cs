using System.Collections.Generic;
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
        private StateTree[] _treesAssets;
        private IStateTree[] _trees;
        private string[] _treesPaths;
        private State[] _statesAssets;
        private IState[] _states;
        private string[] _statesPaths;
        private IState _selectedState;
        private IState _selectedTreeState;
        private IStateTree _currentTree;

        [MenuItem("Window/UI Toolkit/State Machine Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StateMachine>();
            wnd.titleContent = new GUIContent("State Machine");
        }


        // ReSharper disable once UnusedMember.Local
        private void CreateGUI()
        {
            var root = rootVisualElement;
            FindTrees();
            FindStates();
            _currentTree = _trees[0];
            _selectedState = _states[0];
            _selectedTreeState = _states[0];

            _label = new Label();
            _treesDropdown =
                new DropdownField("Trees", _treesPaths.Select(Path.GetFileNameWithoutExtension).ToList(), 0);
            _treesDropdown.RegisterValueChangedCallback(evt => OnPickTree());
            _statesDropdown =
                new DropdownField("States", _statesPaths.Select(Path.GetFileNameWithoutExtension).ToList(), 0);
            _statesDropdown.RegisterValueChangedCallback(evt => OnPickState());
            _stateAddButton = new Button(AddState);
            _stateAddButton.text = "Add state";

            root.Add(_label);
            root.Add(_treesDropdown);
            root.Add(_statesDropdown);
            root.Add(_stateAddButton);
        }

        private void OnInspectorUpdate()
        {
            if (!hasFocus) return;
            FindTrees();
            FindStates();
            UpdateDropdown();
            _label.text = "";
            _label.text += $"{_currentTree.States.Count}";
            
        }

        private void AddState()
        {
            _currentTree.AddState(_selectedState);
            _currentTree.TryConnect(_selectedTreeState.Id, _selectedState.Id);
            
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
            _currentTree = _trees[_treesDropdown.index];
        }

        private void OnPickState()
        {
            _selectedState = _states[_statesDropdown.index];
        }

        private void FindTrees()
        {
            _treesPaths = AssetDatabase.FindAssets("t:StateTree");
            _treesPaths = _treesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _treesAssets = _treesPaths.Select(AssetDatabase.LoadAssetAtPath<StateTree>).ToArray();
            _trees = _treesAssets.Cast<IStateTree>().ToArray();
        }

        private void FindStates()
        {
            _statesPaths = AssetDatabase.FindAssets("t:State");
            _statesPaths = _statesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _statesAssets = _statesPaths.Select(AssetDatabase.LoadAssetAtPath<State>).ToArray();
            _states = _statesAssets.Cast<IState>().ToArray();
        }

        private void Update()
        {
            if (!hasFocus) return;
            _label.transform.position = Mouse.current.position.ReadValue() - position.position;
        }
    }
}