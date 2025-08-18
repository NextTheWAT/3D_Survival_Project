using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    List<Rect> spawnAreas;

    public void Awake()
    {
        spawnAreas = new List<Rect>();
    }
}
