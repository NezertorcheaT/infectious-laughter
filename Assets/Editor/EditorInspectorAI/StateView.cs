using System;
using System.Collections.Generic;
using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor.EditorInspectorAI
{
    public class StateView : Node
    {
        public StateTree.StateForList State;
        public IStateTree<State> Tree;

        public Port Input;
        public List<Port> Outputs;
        public Port NewNext;

        public event Action<StateView> OnStateSelected;
        public event Action<StateView> OnStateUnselected;

        public static readonly string NewOutputText = "newOutput";
        public static readonly string AdvUXML = "Assets/Editor/EditorInspectorAI/NodeViewAdv.uxml";

        private SerializedProperty _name;
        private VisualElement _fieldsContainer;
        private IGlobalParameterNodeStateTree<State, Tuple<Vector2>> _parameterTree;
        private InspectorElement _inspector;
        private UnityEditor.Editor _editor;
        private IStateTreeWithEdits EditableTree;


        public new class UxmlFactory : UxmlFactory<StateView, UxmlTraits>
        {
        }

        public StateView()
        {
            CreateInputPorts();
            CreateOutputPorts();
        }

        public StateView(StateTree.StateForList state, IStateTree<State> tree)
        {
            Outputs = new List<Port>();
            State = state;
            Tree = tree;
            _parameterTree = tree as IGlobalParameterNodeStateTree<State, Tuple<Vector2>>;
            /*foreach (var element in titleContainer.Children())
            {
                Debug.Log(element);
            }*/

            viewDataKey = state.id;

            CreateInputPorts();
            CreateOutputPorts();

            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AdvUXML);
            ui.CloneTree(mainContainer);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AiInspector.USS);
            styleSheets.Add(styleSheet);

            _fieldsContainer = this.Q<VisualElement>("FieldsContainer");
            title = State.state.Name;

            UpdateParameters();
            InitializeEditor();
        }


        public void DeinitializeEditor()
        {
            _fieldsContainer?.Clear();
            Object.DestroyImmediate(_editor);
        }

        public void InitializeEditor()
        {
            _fieldsContainer.Clear();

            if (State.state is not IEditableState editableState)
            {
                _fieldsContainer.RemoveFromHierarchy();
                return;
            }

            EditableTree = Tree as IStateTreeWithEdits;
            if (EditableTree is null) return;

            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(EditableTree.GetEdit(State.id));
            var container = new IMGUIContainer(() => _editor.OnInspectorGUI());
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AiInspector.USS);
            container.styleSheets.Add(sheet);
            _fieldsContainer.Add(container);
        }

        public void UpdateParameters()
        {
            if (!Tree.IsIdValid(State.id)) return;
            var position = _parameterTree.GetParameters(State.id).Item1;
            style.left = position.x;
            style.top = position.y;
        }


        private void CreateInputPorts()
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (Input is not null)
            {
                Input.portName = string.Empty;
                inputContainer.Add(Input);
            }
        }

        private void CreateOutputPorts()
        {
            foreach (var next in Tree.GetNextsTo(State.id))
            {
                var output = InstantiatePort(
                    Orientation.Horizontal,
                    Direction.Output,
                    Port.Capacity.Single,
                    typeof(bool)
                );
                if (output is not null)
                {
                    output.portName = next;
                    outputContainer.Add(output);
                    Outputs.Add(output);
                }
            }

            NewNext = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(bool)
            );
            if (NewNext is null) return;
            NewNext.portName = NewOutputText;
            outputContainer.Add(NewNext);
        }

        public override void OnSelected()
        {
            OnStateSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            OnStateUnselected?.Invoke(this);
        }

        protected override void ToggleCollapse()
        {
            base.ToggleCollapse();
            _fieldsContainer.style.display = new StyleEnum<DisplayStyle>(
                _fieldsContainer.style.display.value == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None
            );
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            _parameterTree?.TrySetParameters(
                State.id,
                new Tuple<Vector2>(
                    new Vector2(newPos.xMin, newPos.yMin)
                )
            );
            UpdateParameters();
        }
    }
}