using System.Collections.Generic;
using System.IO;
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

            _root.Add(ui);
        }

        private void OnInspectorUpdate()
        {
            if (!hasFocus) return;
            FindTrees();
            FindStates();
            UpdateDropdown();
            UpdateNodeElement();
            UpdateNodeElementPositions();

            if (_trees.Length != 0) return;
            _treesDropdown.SetEnabled(false);
            _statesDropdown.SetEnabled(false);
        }

        private void AddState()
        {
            CurrentTree.AddState(SelectedState);
            CurrentTree.TryConnect(_selectedTreeStateID, SelectedState.Id);

            FindTrees();
            FindStates();
        }

        private void UpdateNodeElementPositions()
        {
            var i = 0;
            foreach (var element in _nodeElements)
            {
                element.style.flexShrink = 0;
                element.style.flexGrow = 0;
                //element.style.position = Position.Absolute;
                //element.style.top = 50 + element.contentRect.height * i;
                //element.transform.position = (Vector2) _nodes.transform.position + new Vector2(50, 50 + element.contentRect.height * i);
                i++;
            }
        }

        private void UpdateNodeElement()
        {
            _nodeElements = new List<NodeElement>();
            foreach (var id in CurrentTree.Ids)
            {
                var nodeElement = new NodeElement();
                var state = CurrentTree.GetState(id);
                nodeElement.State = state;
                nodeElement.generateVisualContent += nodeElement.DrawCanvas;
                nodeElement.clicked += () => { OnCurrentStateSelected(id); };

                _nodeElements.Add(nodeElement);
            }

            _nodes.Clear();
            foreach (var element in _nodeElements)
            {
                _nodes.Add(element);
            }
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

        private void OnCurrentStateSelected(int id)
        {
            _selectedTreeStateID = id;
            Debug.Log(id);
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

            if (_currentTreeID >= _treesPaths.Length) _currentTreeID = 0;
        }

        private void FindStates()
        {
            _statesPaths = AssetDatabase.FindAssets("t:State");
            _statesPaths = _statesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _states = _statesPaths.Select(AssetDatabase.LoadAssetAtPath<State>).ToArray();
        }

        /*private void Update()
        {
            if (!hasFocus) return;
            _label.transform.position = Mouse.current.position.ReadValue() - (Vector2) _nodes.transform.position - position.position;
        }*/
    }
}