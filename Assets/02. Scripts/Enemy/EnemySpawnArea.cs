using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnAreaData
{
    public int id;
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public int maxMonster;
}

public class EnemySpawnArea : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterPrefab;

    [SerializeField] private List<GameObject> spawnList = new();
    [SerializeField] private List<SpawnAreaData> spawnAreaData = new();
    
    public void Awake()
    {

    }
}
