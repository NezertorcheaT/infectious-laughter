using System;
using System.IO;
using System.Linq;
using Entity.States;
using Levels.StoryNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Editor.EditorStoryNodes
{
    public class NodeView : Node
    {
        private class NodeToggle : Toggle
        {
            public Action<bool> OnClick;

            public bool noEventValue
            {
                get => base.value;
                set => base.value = value;
            }

            public override bool value
            {
                get => base.value;
                set
                {
                    base.value = value;
                    Debug.Log($"value {base.value}");
                    OnClick?.Invoke(base.value);
                }
            }

            protected override void ToggleValue()
            {
                base.ToggleValue();
                //Debug.Log("ToggleValue");
                //OnClick?.Invoke(noEventValue);
            }
        }

        public StoryTree.NodeForList Node;
        public IStateTree<StoryTree.Node> Tree;

        public IGlobalParameterNodeStateTree<StoryTree.Node, Tuple<Vector2, Color, string, int, bool>>
            ParameterTree;

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
                Tree as IGlobalParameterNodeStateTree<StoryTree.Node, Tuple<Vector2, Color, string, int, bool>>;
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
                NameField[0].style.backgroundColor.value.r,
                NameField[0].style.backgroundColor.value.g,
                NameField[0].style.backgroundColor.value.b, 0f
            ));
            NameField[0].style.borderRightColor = new StyleColor(new Color(
                NameField[0].style.borderRightColor.value.r,
                NameField[0].style.borderRightColor.value.g,
                NameField[0].style.borderRightColor.value.b, 0f
            ));
            NameField[0].style.borderTopColor = new StyleColor(new Color(
                NameField[0].style.borderTopColor.value.r,
                NameField[0].style.borderTopColor.value.g,
                NameField[0].style.borderTopColor.value.b, 0f
            ));
            NameField[0].style.borderLeftColor = new StyleColor(new Color(
                NameField[0].style.borderLeftColor.value.r,
                NameField[0].style.borderLeftColor.value.g,
                NameField[0].style.borderLeftColor.value.b, 0f
            ));
            NameField[0].style.borderBottomColor = new StyleColor(new Color(
                NameField[0].style.borderBottomColor.value.r,
                NameField[0].style.borderBottomColor.value.g,
                NameField[0].style.borderBottomColor.value.b, 0f
            ));

            titleContainer.hierarchy.Insert(0, NameField);

            titleContainer.hierarchy.Insert(0, NameField);
            titleContainer.Remove(titleContainer.Q<Label>("title-label"));
            titleContainer.hierarchy.Insert(0, _shopToggle);
            titleContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.RowReverse);

            title = node.visualName;
            _shopToggle.noEventValue = Node.hasShop;
            _shopToggle.OnClick += OnToggleClick;

            UpdateParameters();
        }

        private void OnToggleClick(bool b)
        {
            Debug.Log("OnToggleClick");
            SetParameters();
        }

        public override string title
        {
            get => NameField.value;
            set => NameField.value = value;
        }

        public void UpdateParameters()
        {
            if (!Tree.IsIdValid(Node.id)) return;
            var (position, visualColor, visualName, scene, shop) = ParameterTree.GetParameters(Node.id);
            ColorField.value = visualColor;
            NameField.value = visualName;
            title = visualName;
            if (scene <= -1)
                SceneField.value = null;
            else
                SceneField.value = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneUtility.GetScenePathByBuildIndex(scene));
            style.left = position.x;
            style.top = position.y;
            _shopToggle.noEventValue = shop;
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
                new Tuple<Vector2, Color, string, int, bool>(
                    Node.visualPosition,
                    ColorField.value,
                    NameField.value,
                    SceneField.value is null
                        ? -1
                        : SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(SceneField.value)),
                    _shopToggle.value
                )
            );
            UpdateParameters();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            SetParameters();

            ParameterTree?.TrySetParameters(
                Node.id,
                new Tuple<Vector2, Color, string, int, bool>(
                    new Vector2(newPos.xMin, newPos.yMin),
                    Node.visualColor,
                    Node.visualName,
                    Node.scene,
                    Node.hasShop
                )
            );
            UpdateParameters();
        }
    }
}