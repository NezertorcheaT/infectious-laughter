using System.Linq;
using Entity.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Editor
{
    public class StateMachine : EditorWindow
    {
        private Label _label;
        private StateTree[] _treesAssets;
        private IStateTree[] _trees;
        private string[] _treesPaths;

        [MenuItem("Window/UI Toolkit/State Machine Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StateMachine>();
            wnd.titleContent = new GUIContent("State Machine");
        }


        // ReSharper disable once UnusedMember.Local
        private void CreateGUI()
        {
            var root = rootVisualElement;

            _label = new Label();
            root.Add(_label);
        }

        private void OnInspectorUpdate()
        {
            if (!hasFocus) return;
            FindTrees();
            _label.text = "";
            foreach (var tree in _trees)
            {
                _label.text += $"\n{tree.States.Count}";
            }
        }

        private void FindTrees()
        {
            _treesPaths = AssetDatabase.FindAssets("t:StateTree");
            _treesPaths = _treesPaths.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            _treesAssets = _treesPaths.Select(AssetDatabase.LoadAssetAtPath<StateTree>).ToArray();
            _trees = _treesAssets.Cast<IStateTree>().ToArray();
        }

        private void Update()
        {
            if (!hasFocus) return;
            _label.transform.position = Mouse.current.position.ReadValue() - position.position;
        }
    }
}