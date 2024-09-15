using Entity.States;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.EditorInspectorAI
{
    [CustomEditor(typeof(StateTree))]
    public class AiInspector : UnityEditor.Editor
    {
        public const string USS = "Assets/Editor/EditorInspectorAi/StoryView.uss";
        public const string UXML = "Assets/Editor/EditorInspectorAi/StoryView.uxml";
        private IStateTree<State> _tree;
        private IUpdatableAssetStateTree<State> _updatableTree;
        private ToolbarButton _saveButton;
        private AiTreeView _treeView;
        private Foldout _defaultInspector;
        private VisualElement _root;
        private string _saveButtonText;

        private void OnEnable()
        {
            Update();
            if (_treeView is null) return;
            if (
                ReferenceEquals(serializedObject.targetObject, _tree) &&
                _treeView.Populated
            ) return;
            if (serializedObject.targetObject is StateTree tree)
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
            _defaultInspector = _root.Q<Foldout>("DefaultInspector");
            _treeView = _root.Q<AiTreeView>("Graph");

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