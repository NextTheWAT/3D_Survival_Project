using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
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
        GameManager.Instance.inventoryManager.AddItem(data, 1);
        Destroy(gameObject);
        Debug.Log($"{data.id} 상호작용됨!");
    }
}
