using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ResourceObject : MonoBehaviour, IInteractable
{
    public ResourceData data;
    public int remainingAmount;

    private ResourceContainer container;

    private void Awake()
    {
        container = GetComponentInParent<ResourceContainer>();
    }
    public void Init()
    {
        Debug.Log("Init");
        remainingAmount = data.maxAmount;
    }

    public string GetInteractPrompt()
    {
        // To do
        // format string data for UI prompt
        return string.Empty;
    }

    public void OnInteract()
    {
        // To do
        // Called when the player interacts with this resource object.
        Debug.Log($"{data.name} 맞음!");
        TryHarvestAndSpawn();
    }
    public bool TryHarvestAndSpawn()
    {
        if (TryHarvest(out int itemId))
        {
            SpawnResource(itemId);
            return true;
        }
        else
        {
            Debug.Log("Out of resource");
            return false;
        }
    }
    public bool TryHarvest(out int itemId)
    {
        if (remainingAmount > 0)
        {
            itemId = HarvestResource();
            return true;
        }

        // failed to harvest
        HandleDepletion();
        itemId = 0;
        return false;
    }
    private void SpawnResource(int itemId)
    {
        if (itemId == 0) return;

        // 데이터베이스에서 아이템 정보 가져오기
        var itemData = TestManager.Instance.itemDatabase.GetItemById(itemId);
        if (itemData == null)
        {
            Debug.LogWarning($"Item ID {itemId} not found!");
            return;
        }

        // 드랍 위치 계산
        Vector3 dropPos = GetDropPosition();

        // 아이템 생성
        Instantiate(itemData.inGamePrefab, dropPos, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    public Vector3 GetDropPosition() 
    {
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)); return transform.position + randomOffset; 
    }

    private int HarvestResource()
    {
        int itemId = data.GetAnItemIdByDropRate();
        if (--remainingAmount <= 0)
        {
            container.DeactivateResource(this);
        }
        
        return itemId;
    }

    public void HandleDepletion()
    {
        // to do // for now, i dont think this will be called.
        // Handle Depletion... 
    }
}
