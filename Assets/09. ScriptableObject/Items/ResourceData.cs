using System;
using UnityEngine;

[Serializable]
public class DropItem
{
    public int itemId;
    [Range(0f, 1f)]
    public float dropRate; 
    public void OnValidate()
    {
        dropRate = Mathf.Clamp(dropRate, 0f, 1f);
    }
}
[CreateAssetMenu(fileName = "Resource_", menuName = "Resource/New Resource")]
public class ResourceData : ScriptableObject
{
    [Header("Infomation")]
    public string displayName;    // ya: wondering what if just not using displayName and description. maybe for prompt text?
    public string description;

    [Header("Source")]
    public GameObject resourcePrefab;   // prefab reference

    [Header("Attributes")]    // ya: any restriction like tool types
    public DropItem[] dropItems;
    public int maxAmount;
    public float respawnTime;

    public int GetAnItemIdByDropRate()
    {
        float total = 0f;
        foreach (var drop in dropItems) total += drop.dropRate;

        float rand = UnityEngine.Random.value * total;
        foreach (var drop in dropItems)
        {
            rand -= drop.dropRate;
            if (rand <= 0f)
                return drop.itemId;
        }

        return dropItems[dropItems.Length - 1].itemId; // fallback: return last item in the array.
    }
}
