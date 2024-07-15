using Entity.States;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.EditorAI
{
    public class InspectorView : VisualElement
    {
        public IStateTree<State> Tree;
        public IStateTreeWithEdits EditableTree;
        private InspectorElement _inspector;
        private UnityEditor.Editor _editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {
        }

        public InspectorView()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachine.USS);
            styleSheets.Add(styleSheet);
        }

        public void DeinitializeState(StateTree.StateForList state, IStateTree<State> tree)
        {
            Clear();
            Object.DestroyImmediate(_editor);
        }

        public void InitializeState(StateTree.StateForList state, IStateTree<State> tree)
        {
            Clear();

            if (!(state.state is IEditableState editableState)) return;
            Tree = tree;
            EditableTree = Tree as IStateTreeWithEdits;
            if (EditableTree is null) return;

            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(EditableTree.GetEdit(state.id));
            var container = new IMGUIContainer(() => _editor.OnInspectorGUI());
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachine.USS);
            container.styleSheets.Add(sheet);
            Add(container);
        }
    }
}