using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameManager();
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

    [Header("Player")]
    public PlayerCondition playerCondition;
    public Transform playerPosition;

    [Header("Item Inventory")]
    public InventoryManager inventoryManager;
    public ItemDatabase itemDatabase;
    public InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.Open();
            Debug.Log("Activate UI");
        }
    }
    //void ResourceHarvestTest(ResourceObject resourceObject)
    //{
    //    if (resourceObject.TryHarvestAndSpawn())
    //    {
    //        Debug.Log("Spawned the resource.");
    //    }
    //    else
    //    { 
    //        Debug.Log("Out of resource");
    //    }
    //}
}
