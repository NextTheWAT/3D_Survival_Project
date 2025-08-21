using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    
    [Header("Infomation")]
    public int id; //To do: double check if unity has any API for automatic id generation.
    public string displayName;
    public string description;
    public int maxStack;

    [Header("Source")]
    public Sprite icon;
    public GameObject inGamePrefab;   // prefab reference

}
