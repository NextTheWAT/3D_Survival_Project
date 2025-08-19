using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InventorySlotData
{
    public ItemData itemData;
    public int count;
}
public class InventoryManager : MonoBehaviour
{
    private static InventoryModel model = new InventoryModel();    //solo player
    private IInventoryMediator mediator;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        //testcode
        model.AddItem(1);
        model.AddItem(2);
    }
    
    public void SetMediator(IInventoryMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddItem(int itemId)
    {
        model.AddItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void RemoveItem(int itemId)
    {
        model.RemoveItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void DropItem(int itemId)
    {
        if(model.GetAmountById(itemId) > 0)
        {
            RemoveItem(itemId);
            mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
            // to do: instantiate new item. player's drop position. or somewhere.
            // Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("Dropped: " + itemId);
        }
    }
    public void UseItem(int itemId)
    {
        RemoveItem(itemId);
        Debug.Log($"Used item {itemId}");
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());

    }

    public int GetItemAmount(int itemId) => model.GetAmountById(itemId);
    public List<int> GetAllItemIds() => model.GetAllIds();
    public ItemData GetItemDataById(int itemId) => itemDatabase.GetItemById(itemId);
    public List<ItemData> GetAllItemData()
    {
        return model.GetAllIds()
                    .Select(id => itemDatabase.GetItemById(id))
                    .ToList();
    }
    public List<InventorySlotData> GetSlotDatas()
    {
        Dictionary<int, int> counts = new();

        foreach (var id in model.GetAllIds())
        {
            if (!counts.ContainsKey(id))
                counts[id] = 0;
            counts[id]++;
        }

        List<InventorySlotData> result = new();
        foreach (var kvp in counts)
        {
            var itemData = itemDatabase.GetItemById(kvp.Key);

            int remaining = kvp.Value;

            while (remaining > 0)
            {
                int stackSize = itemData.maxStack > 0 ? Mathf.Min(itemData.maxStack, remaining) : 1;

                result.Add(new InventorySlotData
                {
                    itemData = itemData,
                    count = stackSize
                });

                remaining -= stackSize;
            }
        }

        return result;
    }
}
