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
            title = node.visualName;
            viewDataKey = node.id;

            CreateInputPorts();
            CreateOutputPorts();
            
            var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AdvUXML);

            ui.CloneTree(mainContainer);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StoryInspector.USS);
            styleSheets.Add(styleSheet);
            
            ColorField = this.Q<ColorField>("Color");
            NameField = this.Q<TextField>("Name");
            SceneField = this.Q<ObjectField>("Scene");
            _background = this.Q<VisualElement>("BG");
            
            UpdateParameters();
        }

        public void UpdateParameters()
        {
            var (position, visualColor, visualName, scene) = ParameterTree.GetParameters(Node.id);
            ColorField.value = visualColor;
            NameField.value = visualName;
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
            OnStateSelected?.Invoke(this);
            UpdateParameters();
        }

        public override void OnUnselected()
        {
            OnStateUnselected?.Invoke(this);
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