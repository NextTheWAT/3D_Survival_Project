using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryModel
{
    private List<int> inventory = new List<int>();  // collect itemIds owned

    public void AddItem(int itemId) => inventory.Add(itemId);
    public void RemoveItem(int itemId)
    {
        if(inventory.Contains(itemId))
        {
            inventory.Remove(itemId);
            Debug.Log("Item Removed");
        }
        else
        {
            Debug.Log(itemId + " is not in your inventory.");
        }
    }
    public int GetAmountById(int itemId) => inventory.FindAll(x => x == itemId).Count;
    public List<int> GetAllIds() => new List<int>(inventory);
}
