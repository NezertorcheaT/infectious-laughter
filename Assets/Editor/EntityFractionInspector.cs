using System;
using System.Linq;
using Entity.Abilities;
using Entity.Relationships;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(EntityFraction))]
    public class EntityFractionInspector : UnityEditor.Editor
    {
        private class FractionDropdownField : DropdownField
        {
            public event Action<int> OnChange;

            public override string value
            {
                get => base.value;
                set
                {
                    base.value = value;
                    OnChange?.Invoke(index);
                }
            }
        }

        private Type[] _types;
        private SerializedProperty _property;

        public override VisualElement CreateInspectorGUI()
        {
            _types = TypeCache.GetTypesDerivedFrom<Fraction>().ToArray();
            _property = serializedObject.FindProperty("type");

            var root = new VisualElement();

            var dropDown = new FractionDropdownField();
            dropDown.choices = _types.Select(i => i.Name.Replace("Fraction", "")).ToList();
            dropDown.label = "Fraction";
            var ints = _types.Select((i, ind) => i.AssemblyQualifiedName == _property.stringValue ? ind : -1);
            if (ints.Any(i => i != -1))
                dropDown.index = ints.First(i => i != -1);
            else
                dropDown.index = 0;
            dropDown.OnChange += OnChange;

            root.Add(dropDown);
            return root;
        }

        private void OnChange(int index)
        {
            _property.stringValue = _types[index].AssemblyQualifiedName;
            serializedObject.ApplyModifiedProperties();
        }
    }
}