using System;
using NaughtyAttributes;
using UnityEngine;
using CustomHelper;
using Random = System.Random;

namespace Levels
{
    public class LevelGenerator : MonoBehaviour
    {
        [Serializable]
        private class PrefabData
        {
            public GameObject groundPrefab;
            public int tilesLength;
            public int changeHeight;
        }

        [SerializeField] private string seed;
        [SerializeField] private int numberOfPrefabs;

        [Space(10), SerializeField] private PrefabData levelStartPrefab;
        [SerializeField] private PrefabData levelEndPrefab;
        [SerializeField] private PrefabData[] levelPrefabsData;

        [Space(10), SerializeField] private bool usePresets;

        [EnableIf("usePresets"), SerializeField]
        private PrefabData[] levelPresetsData;

        private void Start()
        {
            var prefabsNum = levelPrefabsData.Length;
            var presetsNum = levelPresetsData.Length;
            var generatorPosition = Vector2Int.zero;

            var addedPrefabs = usePresets ? 1 : 0;
            var presetID = 0;

            var random = new Random(seed.GetHashCode());

            Instantiate(levelStartPrefab.groundPrefab, generatorPosition.ToVector3(), Quaternion.Euler(0, 0, 0));
            generatorPosition += new Vector2Int(levelStartPrefab.tilesLength, levelStartPrefab.changeHeight);

            for (var i = 0; i < numberOfPrefabs; i++)
            {
                var prefabID = random.Next(0, prefabsNum + addedPrefabs);
                Debug.Log(prefabID);

                if (prefabID > prefabsNum - 1)
                {
                    Instantiate(
                        levelPresetsData[presetID].groundPrefab,
                        generatorPosition.ToVector3(),
                        Quaternion.identity
                    );
                    generatorPosition += new Vector2Int(
                        levelPresetsData[presetID].tilesLength,
                        levelPresetsData[presetID].changeHeight
                    );

                    presetID++;
                    if (presetID > presetsNum - 1) addedPrefabs = 0;

                    prefabID = random.Next(0, prefabsNum);
                }

                Instantiate(
                    levelPrefabsData[prefabID].groundPrefab,
                    generatorPosition.ToVector3(),
                    Quaternion.identity
                );
                generatorPosition += new Vector2Int(
                    levelPrefabsData[prefabID].tilesLength,
                    levelPrefabsData[prefabID].changeHeight
                );
            }

            Instantiate(levelEndPrefab.groundPrefab, generatorPosition.ToVector3(), Quaternion.identity);
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Vector3 ToVector3(this Vector2 vec) => new Vector3(vec.x, vec.y, 0);
        public static Vector3Int ToVector3(this Vector2Int vec) => new Vector3Int(vec.x, vec.y, 0);
    }
}