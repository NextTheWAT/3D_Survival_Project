using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentModel
{
    private Dictionary<EquipType, EquipItemData> equippedItems = new ();  // collect itemIds equipped

    public void EquipItem(EquipItemData item)
    {
        if (item == null) return;

        //디버그 출력용
        if(equippedItems.ContainsKey(item.type)) Debug.Log("current Equip for " + item.type + ": " + item.name);
        equippedItems[item.type] = item; //기존 장비 덮어쓰기
        Debug.Log("new Equip for " + item.type + ": " + item.name);
    }
    public void UnequipItem(EquipItemData item)
    {
        if (equippedItems.ContainsKey(item.type))
        {
            equippedItems.Remove(item.type);
            Debug.Log("Unequipped: " + item.name);
        }
        else
        {
            Debug.Log(item.name + " is not equipped");
        }
    }
    public int GetTotalEnhanceAmount(EnhanceType enhanceType)
    {
        int total = 0;
        foreach (var item in equippedItems.Values)
        {
            if (item.enhancements == null) continue;

            foreach (var enhance in item.enhancements)
            {
                if (enhance.type == enhanceType)
                    total += enhance.amount;
            }
        }
        return total;
    }
    public bool IsEquippedById(int id)
    {
        foreach (var item in equippedItems.Values)
        {
            if (item != null && item.id == id)
                return true;
        }
        return false;
    }
    public bool TryGetEquippedItem(EquipType type, out EquipItemData item)
    {
        return equippedItems.TryGetValue(type, out item);
    }

    //for testing
    public Dictionary<EquipType, EquipItemData> GetEquippedItems()
    {
        return new Dictionary<EquipType, EquipItemData>(equippedItems);
    }
}