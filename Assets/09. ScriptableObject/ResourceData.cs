using System;
using UnityEngine;

[Serializable]
public class DropItem
{
    public int itemId;
    public float dropRate;
}
[CreateAssetMenu(fileName = "ResourceData", menuName = "New Resource")]
public class ResourceData : ScriptableObject
{
    [Header("Infomation")]
    public string displayName;    // ya: wondering what if just not using displayName and description. maybe for prompt text?
    public string description;

    [Header("Source")]
    public GameObject resourcePrefab;   // prefab reference

    [Header("Attributes")]    // ya: any restriction like tool types
    public int maxAmount;
    public float respawnTime;
}
