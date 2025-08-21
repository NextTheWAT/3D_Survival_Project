using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SpawnAreaData
{
    public int id;
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public int maxMonster;
    public List<GameObject> spawnList = new(); //몬스터들 목록
    public List<GameObject> spawnedMonster = new();
}

public class EnemySpawnArea : MonoBehaviour
{
    [SerializeField] private List<SpawnAreaData> spawnAreaData = new();

    public void Start()
    {
        SpawnCollider();
    }

    public void MonsterDeath(int id, GameObject monster)
    {
        SpawnAreaData data = spawnAreaData.FirstOrDefault(d => d.id == id);
        if (data != null && data.spawnedMonster.Contains(monster))
        {
            data.spawnedMonster.Remove(monster);
        }
    }

    public void SpawnCollider()
    {
        foreach(SpawnAreaData data in spawnAreaData)
        {
            GameObject triggerObj = new GameObject($"trigger {data.id}");
            triggerObj.transform.SetParent(transform);
            triggerObj.transform.position = data.spawnAreaCenter;

            BoxCollider box = triggerObj.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = data.spawnAreaSize;

            EnemySpawnTrigger temp = triggerObj.AddComponent<EnemySpawnTrigger>();
            temp.Init(this, data.id);
        }
    }

    public void SpawnMonster(int id)
    {
        SpawnAreaData data = spawnAreaData.FirstOrDefault(d => d.id == id);

        //예외처리
        if (data == null) {
            Debug.Log("몬스터 생성 영역 데이터가 없음");
            return;
        }

        if (data.spawnedMonster.Count >= data.maxMonster) {
            Debug.Log("이미 최대치까지 생성 완료");
            return;
        }

        if (data.spawnList.Count == 0) {
            Debug.Log("몬스터 리스트가 비었음");
        }

        //생성
        Vector3 center = data.spawnAreaCenter;
        Vector3 size = data.spawnAreaSize;

        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomY = Random.Range(-size.y / 2, size.y / 2);

        Vector3 spawnPosition = center + new Vector3(randomX, 0, randomY);

        GameObject randPrefab = data.spawnList[Random.Range(0, data.spawnList.Count)];
        GameObject newMonster = Instantiate(randPrefab, spawnPosition, Quaternion.identity);

        if(newMonster.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.SetOwner(this, id);
        }

        data.spawnedMonster.Add(newMonster);
        Debug.Log($"{data.id} 영역의 {newMonster.name} 생성 완료. 전체 생성까지 남은 몬스터 수 {data.maxMonster - data.spawnedMonster.Count}");
    }


    public void DeSpawnMonster(int id)
    {
        SpawnAreaData data = spawnAreaData.FirstOrDefault(d => d.id == id);

        if (data == null)
        {
            Debug.Log("몬스터 생성 영역 데이터가 없음");
            return;
        }

        if (data.spawnedMonster.Count == 0)
        {
            Debug.Log("활성화된 몬스터 없음");
            return;
        }

        foreach (GameObject monster in data.spawnedMonster)
        {
            Destroy(monster);               
        }
        data.spawnedMonster.Clear();
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        foreach (SpawnAreaData data in spawnAreaData)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawWireCube(data.spawnAreaCenter, data.spawnAreaSize);

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.alignment = TextAnchor.UpperCenter;
            Handles.Label(data.spawnAreaCenter, $"ID: {data.id}", style);
        }
    }
#endif
}
