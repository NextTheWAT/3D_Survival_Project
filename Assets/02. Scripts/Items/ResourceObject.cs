using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour, IInteractable
{
    public ResourceData data;
    public int remainingAmount;

    public void Init()
    {
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

    // + TryHarvest: bool (out itemId);
    //+ HarvestResource: int;
    //+ HandleDepletion: void;
    //+ Deactivate: void;
    //+ RespawnCoroutine: IEnumerator;

}
