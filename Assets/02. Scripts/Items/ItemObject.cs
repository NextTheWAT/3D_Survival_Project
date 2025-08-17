using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemDataBase data;
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
}
