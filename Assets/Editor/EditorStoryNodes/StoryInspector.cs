using Levels.StoryNodes;
using Trees;
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
        private IUpdatableAssetStateTree<StoryTree.Node> _updatableTree;
        private ToolbarButton _saveButton;
        private ColorField _colorField;
        private TextField _nameField;
        private ObjectField _sceneField;
        private StoryTreeView _treeView;
        private Foldout _defaultInspector;
        private VisualElement _root;
        private string _saveButtonText;

        private void OnEnable()
        {
            Update();
            if (_treeView is null) return;
            if (
                ReferenceEquals(serializedObject.targetObject, _tree) &&
                _treeView.Populated &&
                _tree is not null
            ) return;
            if (serializedObject.targetObject is StoryTree tree)
            {
                _tree = tree;
                _updatableTree = tree;
                _treeView.PopulateTree(_tree);
            }

            Update();
        }

        private void OnDestroy()
        {
            EditorApplication.delayCall -= Update;
        }

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML);
            visualTree.CloneTree(_root);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS);
            _root.styleSheets.Add(styleSheet);

            _saveButton = _root.Q<ToolbarButton>("SaveButton");
            _colorField = _root.Q<ColorField>("Color");
            _nameField = _root.Q<TextField>("Name");
            _sceneField = _root.Q<ObjectField>("Scene");
            _defaultInspector = _root.Q<Foldout>("DefaultInspector");
            _treeView = _root.Q<StoryTreeView>("Graph");

            _treeView.Populated = false;
            _saveButtonText = _saveButton.text;
            _saveButton.clicked += UpdateAsset;
            InspectorElement.FillDefaultInspector(_defaultInspector, serializedObject, this);
            EditorApplication.delayCall += Update;
            OnEnable();
            Update();

            return _root;
        }

        private void Update()
        {
            if (_saveButton is null) return;
            _saveButton.text = (_updatableTree is not null && _updatableTree.Unsaved ? "* " : "") + _saveButtonText;
        }

        private void UpdateAsset()
        {
            _updatableTree?.UpdateAsset();
            Update();
        }
    }
}