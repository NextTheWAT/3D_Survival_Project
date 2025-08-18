using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryModel model;    //solo player
    private IInventoryMediator mediator;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        model = new InventoryModel();
    }
    
    public void SetMediator(IInventoryMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddItem(int itemId)
    {
        model.AddItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetAllItemData());
    }
    public void RemoveItem(int itemId)
    {
        model.RemoveItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetAllItemData());
    }
    public void DropItem(int itemId)
    {
        if(model.GetAmountById(itemId) > 0)
        {
            RemoveItem(itemId);
            mediator?.Notify(this, InventoryEventType.ItemDroppRequested, itemId);
            // to do: instantiate new item. player's drop position. or somewhere.
            // Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("Dropped: " + itemId);
        }
    }
    public void UseItem(int itemId)
    {
        RemoveItem(itemId);
        Debug.Log($"Used item {itemId}");
    }

    public int GetItemAmount(int itemId) => model.GetAmountById(itemId);
    public List<int> GetAllItemIds() => model.GetAllIds();
    public List<ItemData> GetAllItemData()
    {
        return model.GetAllIds()
                    .Select(id => itemDatabase.GetItemById(id))
                    .ToList();
    }
}
