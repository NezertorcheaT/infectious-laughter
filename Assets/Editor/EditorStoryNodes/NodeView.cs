using System;
using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Editor.EditorStoryNodes
{
    public class NodeView : Node
    {
        private class NodeToggle : Toggle
        {
            public Action<bool> OnClick;

            protected override void ToggleValue()
            {
                base.ToggleValue();
                OnClick?.Invoke(value);
            }

/*
            public override bool value
            {
                get => base.value;
                set
                {
                    OnClick?.Invoke(value);
                    base.value = value;
                }
            }*/
        }

        public StoryTree.NodeForList Node;
        public IStateTree<StoryTree.Node> Tree;
        public IGlobalParameterNodeStateTree<StoryTree.Node, Tuple<Vector2, Color, string, SceneAsset>> ParameterTree;
        public Port Input;
        public Port Output1;
        public Port Output2;

        public event Action<NodeView> OnStateSelected;
        public event Action<NodeView> OnStateUnselected;

        public static readonly string Output1Text = "end";
        public static readonly string Output2Text = "middle";
        public static readonly string AdvUXML = "Assets/Editor/EditorStoryNodes/NodeViewAdv.uxml";

        public ColorField ColorField;
        public TextField NameField;
        public ObjectField SceneField;

        private SerializedProperty _color;
        private SerializedProperty _name;
        private SerializedProperty _scene;

        private VisualElement _background;
        private VisualElement _fieldsContainer;
        private NodeToggle _shopToggle;


        public new class UxmlFactory : UxmlFactory<NodeView, UxmlTraits>
        {
        }

        public NodeView()
        {
            CreateInputPorts();
            CreateOutputPorts();
        }

        public NodeView(StoryTree.NodeForList node, IStateTree<StoryTree.Node> tree)
        {
            Node = node;
            Tree = tree;
            ParameterTree =
                Tree as IGlobalParameterNodeStateTree<StoryTree.Node, Tuple<Vector2, Color, string, SceneAsset>>;
            /*foreach (var element in titleContainer.Children())
            {
                Debug.Log(element);
            }*/


            viewDataKey = node.id;

            CreateInputPorts();
            CreateOutputPorts();

            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AdvUXML);

            ui.CloneTree(mainContainer);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StoryInspector.USS);
            styleSheets.Add(styleSheet);

            _fieldsContainer = this.Q<VisualElement>("FieldsContainer");
            ColorField = this.Q<ColorField>("Color");
            SceneField = this.Q<ObjectField>("Scene");
            _background = this.Q<VisualElement>("BG");

            _shopToggle = new NodeToggle {label = string.Empty};
            NameField = new TextField {label = string.Empty};
            NameField.style.marginRight = 26;
            NameField[0].style.backgroundColor = new StyleColor(new Color(
                NameField[0].style.color.value.r,
                NameField[0].style.color.value.g,
                NameField[0].style.color.value.b, 0f
            ));
            NameField[0].style.borderRightColor = new StyleColor(new Color(
                NameField[0].style.color.value.r,
                NameField[0].style.color.value.g,
                NameField[0].style.color.value.b, 0f
            ));
            NameField[0].style.borderTopColor = new StyleColor(new Color(
                NameField[0].style.color.value.r,
                NameField[0].style.color.value.g,
                NameField[0].style.color.value.b, 0f
            ));
            NameField[0].style.borderLeftColor = new StyleColor(new Color(
                NameField[0].style.color.value.r,
                NameField[0].style.color.value.g,
                NameField[0].style.color.value.b, 0f
            ));
            NameField[0].style.borderBottomColor = new StyleColor(new Color(
                NameField[0].style.color.value.r,
                NameField[0].style.color.value.g,
                NameField[0].style.color.value.b, 0f
            ));

            titleContainer.hierarchy.Insert(0, NameField);

            titleContainer.hierarchy.Insert(0, NameField);
            titleContainer.Remove(titleContainer.Q<Label>("title-label"));
            titleContainer.hierarchy.Insert(0, _shopToggle);
            titleContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.RowReverse);

            title = node.visualName;
            _shopToggle.OnClick += OnToggleClick;

            UpdateParameters();
        }

        private void OnToggleClick(bool b)
        {
            
        }

        public override string title
        {
            get { return NameField.value; }
            set { NameField.value = value; }
        }

        public void UpdateParameters()
        {
            if (!Tree.IsIdValid(Node.id)) return;
            var (position, visualColor, visualName, scene) = ParameterTree.GetParameters(Node.id);
            ColorField.value = visualColor;
            NameField.value = visualName;
            title = visualName;
            SceneField.value = scene;
            style.left = position.x;
            style.top = position.y;
            _background.style.backgroundColor = visualColor;
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
            Output1 = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(bool)
            );
            Output2 = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(bool)
            );

            if (Output1 is not null)
            {
                Output1.portName = Output1Text;
                outputContainer.Add(Output1);
            }

            if (Output2 is not null)
            {
                Output2.portName = Output2Text;
                outputContainer.Add(Output2);
            }
        }

        public override void OnSelected()
        {
            SetParameters();
            OnStateSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            SetParameters();
            OnStateUnselected?.Invoke(this);
        }

        protected override void ToggleCollapse()
        {
            base.ToggleCollapse();
            _fieldsContainer.style.display = new StyleEnum<DisplayStyle>(
                _fieldsContainer.style.display.value == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None
            );
        }

        public void SetParameters()
        {
            ParameterTree?.TrySetParameters(
                Node.id,
                new Tuple<Vector2, Color, string, SceneAsset>(
                    Node.visualPosition,
                    ColorField.value,
                    NameField.value,
                    SceneField.value as SceneAsset
                )
            );
            UpdateParameters();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            ParameterTree?.TrySetParameters(
                Node.id,
                new Tuple<Vector2, Color, string, SceneAsset>(
                    new Vector2(newPos.xMin, newPos.yMin),
                    ColorField.value,
                    NameField.value,
                    SceneField.value as SceneAsset
                )
            );
            UpdateParameters();
        }
    }
}