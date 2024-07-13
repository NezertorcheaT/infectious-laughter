using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Serializable]
    private class PrefabData
    {
        public GameObject groundPrefab;
        public int tilesLength;
        public int changeHeight;
    }

    [Serializable]
    private class PresetsData
    {
        public GameObject presetPrefab;
        public int tilesLength;
        public int changeHeight;
    }

    [SerializeField] private int numberOfPrefabs;

    [Space(10), SerializeField] private PrefabData levelStartPrefab;
    [SerializeField] private PrefabData levelEndPrefab;
    [SerializeField] private PrefabData[] levelPrefabsData;

    [Space(10), SerializeField] private bool usePresets;
    [EnableIf("usePresets"), SerializeField] private PresetsData[] levelPresetsData;

    void Start()
    {
        int prefabsNum = levelPrefabsData.Length;
        int presetsNum = levelPresetsData.Length;
        Vector2Int generatorPosition = Vector2Int.zero;

        int addedPrefabs = usePresets ? 1 : 0;

        int presetID = 0;

        Instantiate(levelStartPrefab.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
        generatorPosition += new Vector2Int(levelStartPrefab.tilesLength, levelStartPrefab.changeHeight);

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            int prefabID = UnityEngine.Random.Range(0, prefabsNum + addedPrefabs);
            Debug.Log(prefabID);

            if (prefabID > prefabsNum - 1)
            {
                Instantiate(levelPresetsData[presetID].presetPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
                generatorPosition += new Vector2Int(levelPresetsData[presetID].tilesLength, levelPresetsData[presetID].changeHeight);

                presetID++;
                if (presetID > presetsNum - 1) addedPrefabs = 0;

                prefabID = UnityEngine.Random.Range(0, prefabsNum);
            }
            
            Instantiate(levelPrefabsData[prefabID].groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
            generatorPosition += new Vector2Int(levelPrefabsData[prefabID].tilesLength, levelPrefabsData[prefabID].changeHeight);
        }

        Instantiate(levelEndPrefab.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
    }

    static Vector3 Vec2ToVec3(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }
}
