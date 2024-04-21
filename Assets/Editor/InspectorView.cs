using Entity.States;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public IStateTree Tree;
        public IStateTreeWithEdits EditableTree;
        private InspectorElement _inspector;
        private UnityEditor.Editor _editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {
        }

        public InspectorView()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            styleSheets.Add(styleSheet);
        }

        public void DeinitializeState(StateTree.StateForList state, IStateTree tree)
        {
            Clear();
            Object.DestroyImmediate(_editor);
        }

        public void InitializeState(StateTree.StateForList state, IStateTree tree)
        {
            Clear();

            if (!(state.state is IEditableState editableState)) return;
            Tree = tree;
            EditableTree = Tree as IStateTreeWithEdits;
            if (EditableTree is null) return;

            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(EditableTree.GetEdit(state.id));
            var container = new IMGUIContainer(() => _editor.OnInspectorGUI());
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            container.styleSheets.Add(sheet);
            Add(container);
        }
    }
}