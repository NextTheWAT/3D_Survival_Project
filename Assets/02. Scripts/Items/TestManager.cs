using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public ResourceObject resourceObject;
    public Transform dropPosition;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RunTest();
        }

    }
    void RunTest()
    {
        int id = 0;
        if (resourceObject.TryHarvest(out id))
        {
            Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("TryHarvest: " + id);
        }
        else
        { 
            Debug.Log("Out of resource");
        }
    }
}
