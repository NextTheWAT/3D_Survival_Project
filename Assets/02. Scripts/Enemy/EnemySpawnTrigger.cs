using Object.Character.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public EnemySpawnArea owner;
    public int spawnAreaId;

    public void Init(EnemySpawnArea spawner, int id)
    {
        owner = spawner;
        spawnAreaId = id;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("들어감0");
        owner.SpawnMonster(spawnAreaId);
        if (other.CompareTag("Player")) //?
        {
            Debug.Log("들어감");
            owner.SpawnMonster(spawnAreaId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            owner.DeSpawnMonster(spawnAreaId);
        }
    }
}
