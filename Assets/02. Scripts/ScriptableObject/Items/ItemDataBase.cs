using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    public ItemData GetItemById(int id)
    {
        return items.Find(item => item.id == id);
    }

    #if UNITY_EDITOR
    [ContextMenu("Auto Populate Items")]
    private void AutoPopulateItems()
    {
        // 프로젝트 내 모든 ItemData SO 검색
        string[] guids = AssetDatabase.FindAssets("t:ItemData");

        items.Clear();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && !items.Contains(item))
            {
                items.Add(item);
            }
        }

        // 저장 표시
        EditorUtility.SetDirty(this);
        Debug.Log($"ItemDatabase populated with {items.Count} items.");
    }
#endif
}
