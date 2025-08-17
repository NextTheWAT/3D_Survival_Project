using UnityEngine;

public abstract class ItemDataBase : ScriptableObject
{
    
    [Header("Infomation")]
    private int id; //To do: double check if unity has any API for automatic id generation.
    public string displayName;
    public string description;
    public int maxStack;

    [Header("Source")]
    public Sprite icon;
    public GameObject inGamePrefab;   // prefab reference

}
