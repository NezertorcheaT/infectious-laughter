using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Generation
{
    public abstract class GenerationPrefab : MonoBehaviour
    {
        [field: SerializeField] public Tilemap Tilemap { get; private set; }

        public IEnumerable<GameObject> NoneTileChildren
        {
            get
            {
                for (var i = 0; i < noneGridObjectsParent.childCount; i++)
                {
                    yield return noneGridObjectsParent.GetChild(i).gameObject;
                }
            }
        }

        [SerializeField] private Transform noneGridObjectsParent;
    }
}