using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button button;

    public Action onClick;
    
    // InventorySlotData를 받아서 ui 표시하기
    public void Bind(InventorySlotData slotData)
    {
        icon.sprite = slotData.itemData.icon;
        if (countText)  //null 가드
            countText.text = slotData.count > 1 ? slotData.count.ToString() : "";


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}