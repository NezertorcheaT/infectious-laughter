using System;
using System.Collections.Generic;
using UnityEngine;

namespace Outline
{
    [CreateAssetMenu(fileName = "Outlines Container", menuName = "", order = 0)]
    public class OutlinesContainer : ScriptableObject
    {
        [Serializable]
        public struct OutlineType
        {
            public string Path;
            public Sprite Sprite;
        }

        [field: SerializeField] public List<OutlineType> Cache { get; private set; }

        public OutlinesContainer Instance { get; set; }

        public void Initialize()
        {
            Instance ??= this;
        }

        public void Reset()
        {
            Cache = new List<OutlineType>();
            Initialize();
        }
    }
}