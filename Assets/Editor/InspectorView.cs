using Entity.States;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public IStateTree Tree;
        public IStateTreeWithEdits EditableTree;
        private InspectorElement _inspector;

        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {
        }

        public InspectorView()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            styleSheets.Add(styleSheet);
        }

        public void InitializeState(StateTree.StateForList state, IStateTree tree)
        {
            Clear();

            if (!(state.state is IEditableState editableState)) return;
            Tree = tree;
            EditableTree = Tree as IStateTreeWithEdits;
            if (EditableTree is null) return;

            _inspector = new InspectorElement();
            _inspector.Bind(new SerializedObject(EditableTree.GetEdit(state.id)));
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachine.uss");
            _inspector.styleSheets.Add(sheet);
            Add(_inspector);
        }
    }
}