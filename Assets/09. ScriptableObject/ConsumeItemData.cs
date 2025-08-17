using System;
using UnityEngine;


[Serializable]
public class Restoration
{
    public ConsumeType type;
    public int amount;
}

[CreateAssetMenu(fileName = "ConsumeItem", menuName = "New Consume Item")]
public class ConsumeItemData : ItemDataBase
{
    [Header("Attributes")]
    public Restoration[] restorations;
}
