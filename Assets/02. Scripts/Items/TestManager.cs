using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private static TestManager instance;
    public static TestManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new TestManager();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public ItemDatabase itemDatabase;
    public ResourceObject resourceObject;
    public Transform dropPosition;
    public InventoryUI inventoryUI;
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResourceHarvestTest();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.Open();
            Debug.Log("Activate UI");
        }
    }
    void ResourceHarvestTest()
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
