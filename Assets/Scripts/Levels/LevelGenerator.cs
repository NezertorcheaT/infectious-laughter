using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int levelLong;
    [SerializeField, MinValue(0), MaxValue(8)] private int levelRoughness;
    [SerializeField] PrefabData[] wayData;

    float[] heights;
    int prefabNum;

    void Start()
    {
        heights = GetHeights(1, 1);
        prefabNum = wayData.Length;
    }

    void Update()
    {
        int prefabID = UnityEngine.Random.Range(0, prefabNum);
    }

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
}

[Serializable]

class PrefabData
{
    public GameObject groundPrefab;
    public int lenght;
    public int wayGoTo;
}
