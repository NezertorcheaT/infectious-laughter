using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.EditorStoryNodes
{
    public class StoryView : EditorWindow
    {
        public static readonly string USS = "Assets/Editor/EditorStoryNodes/StoryView.uss";
        public static readonly string UXML = "Assets/Editor/EditorStoryNodes/StoryView.uxml";
        private IStateTree<StoryTree.Node> _tree;
        private VisualElement _root;
        private ToolbarButton _saveButton;
        private ToolbarButton _regenButton;
        private Label _stateTreeLabel;
        private string _treeLabelText;

        [MenuItem("Window/Story Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StoryView>();
            wnd.titleContent = new GUIContent("Story View");
        }

        public void CreateGUI()
        {
            RegenerateVisualTreeAsset();
        }

        private void UpdateAsset() => (_tree as IUpdatableAssetStateTree<StoryTree.Node>)?.UpdateAsset();

        private void RegenerateVisualTreeAsset()
        {
            _root = rootVisualElement;

            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML);

            _root.Clear();
            ui.CloneTree(_root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS);
            _root.styleSheets.Add(styleSheet);

            _stateTreeLabel = _root.Q<Label>("GraphViewName");
            _saveButton = _root.Q<ToolbarButton>("SaveButton");
            _regenButton = _root.Q<ToolbarButton>("RegenButton");

            _saveButton.clicked += UpdateAsset;
            _regenButton.clicked += RegenerateVisualTreeAsset;

            OnSelectionChange();
            UpdateAsset();
        }


        private void OnSelectionChange()
        {
            if (Selection.activeObject is IStateTree<StoryTree.Node> tree)
            {
                _tree = tree;
                _treeLabelText = $"Nodes of \"{(_tree as ScriptableObject)?.name}\"";
                OnInspectorUpdate();/*
                _stateTreeView.PopulateTree(tree);
                _stateTreeView.OnStateSelected += a => _inspectorView.InitializeState(a.State, a.Tree);
                _stateTreeView.OnStateUnselected += a => _inspectorView.DeinitializeState(a.State, a.Tree);*/
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
            if (!(_tree is IUpdatableAssetStateTree<StoryTree.Node> updatableAssetStateTree)) return;
            _stateTreeLabel.text = (updatableAssetStateTree.Unsaved ? "* " : "") + _treeLabelText;
        }
    }
}