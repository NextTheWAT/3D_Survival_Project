using System;
using UnityEngine;


[Serializable]
public class Restoration
{
    public ConsumeType type;
    public int amount;
}

[CreateAssetMenu(fileName = "ConsumeItem", menuName = "Item/New Consume Item")]
public class ConsumeItemData : ItemData
{
    [Header("Attributes")]
    public Restoration[] restorations;
}
