using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfPrefabs;

    [Space(10), SerializeField] private PrefabData levelStartPrefab;
    [SerializeField] private PrefabData levelEndPrefab;
    [SerializeField] private PrefabData[] levelPrefabsData;

    void Start()
    {
        int prefabNum = levelPrefabsData.Length;
        Vector2Int generatorPosition = Vector2Int.zero;

        Instantiate(levelStartPrefab.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
        generatorPosition += new Vector2Int(levelStartPrefab.tilesLength, levelStartPrefab.changeHeight);

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            int prefabID = UnityEngine.Random.Range(0, prefabNum);
            Instantiate(levelPrefabsData[prefabID].groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
            generatorPosition += new Vector2Int(levelPrefabsData[prefabID].tilesLength, levelPrefabsData[prefabID].changeHeight);
        }

        Instantiate(levelEndPrefab.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
    }

    Vector3 Vec2ToVec3(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }
}

[Serializable]

class PrefabData
{
    public GameObject groundPrefab;
    public int tilesLength;
    public int changeHeight;
}
