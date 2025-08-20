using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentModel
{
    private Dictionary<EquipType, (EquipItemData item, int slotId)> equippedItems = new();

    public void EquipItem(EquipItemData item, int slotId)
    {
        if (item == null) return;

        //디버그 출력용
        if (equippedItems.ContainsKey(item.type))
            Debug.Log("Current Equip for " + item.type + ": " + equippedItems[item.type].item.name);

        equippedItems[item.type] = (item, slotId); //슬롯 정보와 함께 저장
        Debug.Log("New Equip for " + item.type + ": " + item.name + " (slotId " + slotId + ")");
    }
    public void UnequipItem(EquipItemData item, int slotId)
    {
        if (equippedItems.ContainsKey(item.type) && equippedItems[item.type].slotId == slotId)
        {
            equippedItems.Remove(item.type);
            Debug.Log("Unequipped: " + item.name + " from slot " + slotId);
        }
        else
        {
            Debug.Log(item.name + " (slotId " + slotId + ") is not equipped");
        }
    }
    public int GetTotalEnhanceAmount(EnhanceType enhanceType)
    {
        int total = 0;
        foreach (var pair in equippedItems.Values)
        {
            var item = pair.item;
            if (item.enhancements == null) continue;
            foreach (var enhance in item.enhancements)
            {
                if (enhance.type == enhanceType)
                    total += enhance.amount;
            }
        }
        return total;
    }
    public bool IsEquippedBySlotId(int slotId)
    {
        foreach (var pair in equippedItems.Values)
        {
            if (pair.slotId == slotId) return true;
        }
        return false;
    }
    public bool TryGetEquippedItem(EquipType type, out EquipItemData item)
    {
        if (equippedItems.TryGetValue(type, out var pair))
        {
            item = pair.item;
            return true;
        }
        item = null;
        return false;
    }

    // 테스트용
    public Dictionary<EquipType, (EquipItemData item, int slotId)> GetEquippedItems()
    {
        return new Dictionary<EquipType, (EquipItemData, int)>(equippedItems);
    }
}