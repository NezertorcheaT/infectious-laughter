using System;

namespace Entity.States
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StateEditAttribute : Attribute
    {
    }
}