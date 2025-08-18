using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryModel : MonoBehaviour
{
    private List<int> inventory = new List<int>();  // collect itemIds owned
    public int selectedItemId;

    public void AddItem(int itemId)
    {
        inventory.Add(itemId);
        PrintInventory();
    }
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
        PrintInventory();
    }

    public void DropItem(int itemId)    //thinking about implementing this method in inventory ui or somewhere not here.
    {

    }

    public ItemData GetItemById(int itemId)
    {
        // to do
        // database에서 아이템 골라... return 하기.
        return new ItemData();
    }

    public int GetAmountById(int itemId)
    {
        return inventory.FindAll(x => x == itemId).Count;
    }

    public void ProcessItem(int itemId) //왜 만들었더라? 
    {

    }

    // for test
    void PrintInventory()
    {
        string ids = "";
        foreach (int itemId in inventory)
        {
            ids += itemId + ", ";
        }
        Debug.Log("Current Inventory: " + ids);
    }
}
