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
    public ResourceObject resourceObject2;
    public Transform dropPosition;
    public InventoryUI inventoryUI;
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResourceHarvestTest(resourceObject);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ResourceHarvestTest(resourceObject2);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.Open();
            Debug.Log("Activate UI");
        }
    }
    void ResourceHarvestTest(ResourceObject resourceObject)
    {
        int id = 0;
        if (resourceObject.TryHarvest(out id))
        {
            Vector3 dropPos = resourceObject.GetDropPosition();
            Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPos, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("TryHarvest: " + id);
        }
        else
        { 
            Debug.Log("Out of resource");
        }
    }
}
