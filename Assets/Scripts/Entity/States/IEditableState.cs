using System;
using UnityEngine;

namespace Entity.States
{
    public interface IEditableState
    {
        Type GetTypeOfEdit();

        abstract class Properties : ScriptableObject
        {
            public abstract T Get<T>(string name);
            public abstract void Set<T>(string name, T value);
        }
    }
}