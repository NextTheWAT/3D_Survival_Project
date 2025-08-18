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
