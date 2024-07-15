using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.EditorStoryNodes
{
    [CustomEditor(typeof(StoryTree))]
    public class StoryInspector : UnityEditor.Editor
    {
        public static readonly string USS = "Assets/Editor/EditorStoryNodes/StoryView.uss";
        public static readonly string UXML = "Assets/Editor/EditorStoryNodes/StoryView.uxml";
        private IStateTree<StoryTree.Node> _tree;
        private ToolbarButton _saveButton;
        private ColorField _colorField;
        private TextField _nameField;
        private ObjectField _sceneField;
        private StoryTreeView _treeView;
        private VisualElement _root;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML);
            visualTree.CloneTree(_root);
            _tree = serializedObject.targetObject as IStateTree<StoryTree.Node>;

            _saveButton = _root.Q<ToolbarButton>("SaveButton");
            _colorField = _root.Q<ColorField>("Color");
            _nameField = _root.Q<TextField>("Name");
            _sceneField = _root.Q<ObjectField>("Scene");
            _treeView = _root.Q<StoryTreeView>();

            _saveButton.clicked += UpdateAsset;

            return _root;
        }

        private void UpdateAsset() => (_tree as IUpdatableAssetStateTree<StoryTree.Node>)?.UpdateAsset();
    }
}