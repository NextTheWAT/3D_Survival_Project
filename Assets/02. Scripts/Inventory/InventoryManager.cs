using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
public class InventorySlotData
{
    public ItemData itemData;
    public int count;
}
public class InventoryManager : MonoBehaviour
{
    private static InventoryModel inventoryModel = new InventoryModel();    //solo player
    private static EquipmentModel equipmentModel = new EquipmentModel();    //solo player

    private IInventoryMediator mediator;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        //testcode
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(1);
        inventoryModel.AddItem(2);
        inventoryModel.AddItem(3);
        inventoryModel.AddItem(4);
        inventoryModel.AddItem(4);
    }
    
    public void SetMediator(IInventoryMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddItem(int itemId)
    {
        inventoryModel.AddItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void RemoveItem(int itemId)
    {
        inventoryModel.RemoveItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void DropItem(int itemId)
    {
        if(inventoryModel.GetAmountById(itemId) > 0)
        {
            if(equipmentModel.IsEquippedById(itemId))
                equipmentModel.UnequipItem(itemDatabase.GetItemById(itemId) as EquipItemData);
            RemoveItem(itemId);
            mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
            // to do: instantiate new item. player's drop position. or somewhere.
            // Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("Dropped: " + itemId);
        }
        PrintEquippedItems();
    }
    public void UseItem(int itemId)
    {
        RemoveItem(itemId);
        Debug.Log($"Used item {itemId}");
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());

    }
    public void EquipItem(int itemId)
    {
        var itemData = itemDatabase.GetItemById(itemId) as EquipItemData;
        if (itemData == null) return;

        equipmentModel.EquipItem(itemData);

        //PrintEquippedItems();
    }
    public void UnequipItem(int itemId)
    {
        var itemData = itemDatabase.GetItemById(itemId) as EquipItemData;
        if (itemData == null) return;

        equipmentModel.UnequipItem(itemData);

        //PrintEquippedItems();
    }
    //Test Method
    public void PrintEquippedItems()
    {
        Debug.Log("--Current equipment List--");
        foreach (var item in equipmentModel.GetEquippedItems())
        {
            Debug.Log(item.Value.name);
        }
    }
    public int GetItemAmount(int itemId) => inventoryModel.GetAmountById(itemId);
    public List<int> GetAllItemIds() => inventoryModel.GetAllIds();
    public ItemData GetItemDataById(int itemId) => itemDatabase.GetItemById(itemId);
    public List<ItemData> GetAllItemData()
    {
        return inventoryModel.GetAllIds()
                    .Select(id => itemDatabase.GetItemById(id))
                    .ToList();
    }
    public List<InventorySlotData> GetSlotDatas()
    {
        Dictionary<int, int> counts = new();

        foreach (var id in inventoryModel.GetAllIds())
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

    public bool IsEquipped(int id)
    {
        return equipmentModel.IsEquippedById(id);
    }
}
