using System;
using UnityEngine;

[Serializable]
public class Enhancement
{
    public EnhanceType type;
    public int amount;
}

[CreateAssetMenu(fileName = "EquipItem", menuName = "New Equip Item")]
public class EquipItemData : ItemDataBase
{
    public GameObject equipPrefab;   // prefab reference

    [Header("Attributes")]
    public EquipType type;
    public Enhancement[] enhancements;
}
