using System;
using System.Collections.Generic;
using UnityEngine;

namespace Outline
{
    [CreateAssetMenu(fileName = "Outlines Container", menuName = "", order = 0)]
    public class OutlinesContainer : ScriptableObject
    {
        [Serializable]
        private struct OutlineType
        {
            public string Path;
            public Sprite Sprite;
        }

        [SerializeField] private List<OutlineType> cache;
    }
}