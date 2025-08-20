using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryModel
{
    private List<InventorySlotData> slots = new List<InventorySlotData>();

    public void AddItem(ItemData itemData, int amount = 1)
    {
        int remaining = amount;

        // 기존 슬롯에 스택 가능한 경우 먼저 채우기
        foreach (var slot in slots)
        {
            if (slot.itemData == itemData && itemData.maxStack > 1)
            {
                int canAdd = itemData.maxStack - slot.count;
                int toAdd = Mathf.Min(canAdd, remaining);
                slot.count += toAdd;
                remaining -= toAdd;
                if (remaining <= 0) return;
            }
        }

        // 남은 수량은 새 슬롯으로 추가
        while (remaining > 0)
        {
            int stackSize = itemData.maxStack > 0 ? Mathf.Min(itemData.maxStack, remaining) : 1;
            slots.Add(new InventorySlotData(itemData, stackSize));
            remaining -= stackSize;
        }
    }

    public void RemoveOneItemFromSlot(int slotId)
    {
        var slot = slots.Find(s => s.slotId == slotId);
        if (slot != null)
        {
            slot.count--;
            if (slot.count <= 0)
            {
                slots.Remove(slot); // 슬롯이 비면 제거하기
            }
            Debug.Log($"Removed 1 item from slot {slotId}");
        }
        else
        {
            Debug.Log($"SlotId {slotId} not found.");
        }
    }

    public void RemoveAllItemsById(int itemId)
    {
        slots.RemoveAll(s => s.itemData.id == itemId);
    }

    public int GetAmountByItemId(int itemId)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.itemData.id == itemId) total += slot.count;
        }
        return total;
    }
    public List<InventorySlotData> GetAllSlots() => new List<InventorySlotData>(slots);

    public void Clear()
    {
        slots.Clear();
        InventorySlotData.InitId();
    }
}
