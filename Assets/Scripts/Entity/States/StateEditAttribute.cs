using System;
using System.Reflection;

namespace Entity.States
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StateEditAttribute : Attribute
    {
        public static FieldInfo GetStateEditField(Type type, Type editType)
        {
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType.AssemblyQualifiedName != editType.AssemblyQualifiedName) continue;
                foreach (var attributeData in field.CustomAttributes)
                {
                    if (attributeData.AttributeType.AssemblyQualifiedName ==
                        typeof(StateEditAttribute).AssemblyQualifiedName)
                        return field;
                }
            }

            return null;
        }
    }
}