using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    //public void DropItem(int itemId)    //thinking about implementing this method in inventory ui or somewhere not here.
    //{

    //}

    //public ItemData GetItemById(int itemId)
    //{
    //    // to do
    //    // database에서 아이템 골라... return 하기.
    //    return new ItemData();
    //}

    public int GetAmountById(int itemId) => inventory.FindAll(x => x == itemId).Count;

    //public void ProcessItem(int itemId) //왜 만들었더라? 
    //{

    //}

    public List<int> GetAllIds() => new List<int>(inventory);

    //// for test
    //void PrintInventory()
    //{
    //    string ids = "";
    //    foreach (int itemId in inventory)
    //    {
    //        ids += itemId + ", ";
    //    }
    //    Debug.Log("Current Inventory: " + ids);
    //}
}
