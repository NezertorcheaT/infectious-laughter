using System;
using Entity.States;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class StateMachine : EditorWindow
    {
        private VisualElement _root;
        private VisualElement _nodes;
        private StateTreeView _stateTreeView;
        private InspectorView _inspectorView;
        private ToolbarButton _saveButton;
        private Label _stateTreeLabel;
        private IStateTree _tree;
        private Controls _controls;

        [MenuItem("Window/State Machine Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StateMachine>();
            wnd.titleContent = new GUIContent("State Machine");
        }

        public void CreateGUI()
        {
            _root = rootVisualElement;

            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/StateMachine.uxml");

            ui.CloneTree(_root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            _root.styleSheets.Add(styleSheet);

            _stateTreeView = _root.Q<StateTreeView>();
            _inspectorView = _root.Q<InspectorView>();
            _stateTreeLabel = _root.Q<Label>("GraphViewName");
            _saveButton = _root.Q<ToolbarButton>("SaveButton");

            OnSelectionChange();
            UpdateAsset();

            _saveButton.clicked += UpdateAsset;
        }

        private string _treeLabelText;

        private void UpdateAsset() => (_tree as IUpdatableAssetStateTree)?.UpdateAsset();

        private void OnSelectionChange()
        {
            if (Selection.activeObject is IStateTree tree)
            {
                _tree = tree;
                _treeLabelText = $"Nodes of \"{(_tree as ScriptableObject)?.name}\"";
                OnInspectorUpdate();
                _stateTreeView.PopulateTree(tree);
                return;
            }

            if (_tree is null)
            {
                _treeLabelText = "";
                OnInspectorUpdate();
            }
        }

        private void OnInspectorUpdate()
        {
            if (!(_tree is IUpdatableAssetStateTree updatableAssetStateTree)) return;
            _stateTreeLabel.text = (updatableAssetStateTree.Unsaved ? $"* " : "") + _treeLabelText;
        }
    }
}