using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int levelLong;
    [Space(10), SerializeField] private PrefabData levelStart;
    [SerializeField] private PrefabData levelEnd;
    [SerializeField] private PrefabData[] GeneratorData;

    float[] heights;
    int prefabNum;
    Vector2Int generatorPosition;

    void Start()
    {
        prefabNum = GeneratorData.Length;

        Instantiate(levelStart.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
        generatorPosition += new Vector2Int(levelStart.length, levelStart.UpDownTo);

        for (int i = 0; i < levelLong; i++)
        {
            int prefabID = UnityEngine.Random.Range(0, prefabNum);
            Instantiate(GeneratorData[prefabID].groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
            generatorPosition += new Vector2Int(GeneratorData[prefabID].length, GeneratorData[prefabID].UpDownTo);
        }

        Instantiate(levelEnd.groundPrefab, Vec2ToVec3(generatorPosition), Quaternion.Euler(0, 0, 0));
    }

    void Update()
    {
    }

    Vector3 Vec2ToVec3(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

    /*
     * На случай, если будет через шум перлина
    
    private float[] GetHeights(int worldWidth, float steps)
    {
        var ints = new List<float>();
        var r = UnityEngine.Random.Range(-100f, 100f);
        for (var i = 0; i < worldWidth; i++)
        {
            ints.Add(steps *
                     (int)(Mathf.PerlinNoise(
                                (float)i / worldWidth * 5 + r,
                                (float)i / worldWidth * 5 + r)
                            * 8f));
        }

        return ints.ToArray();
    }
    */
}

[Serializable]

class PrefabData
{
    public GameObject groundPrefab;
    public int length;
    public int UpDownTo;
}
