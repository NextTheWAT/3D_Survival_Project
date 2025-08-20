using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : BaseUI
{
    [Header("Slots")]
    [SerializeField] InventorySlotUI inputSlot;
    [SerializeField] InventorySlotUI outputSlot;

    [Header("Texts (optional)")]
    [SerializeField] TextMeshProUGUI inputNameText;
    [SerializeField] TextMeshProUGUI outputNameText;

    [Header("Buttons")]
    [SerializeField] Button craftButton;
    [SerializeField] Button closeButton;

    private ItemData inputItem;
    private ItemData outputItem;   // 캐시해두면 버튼 클릭 시에도 바로 참조 가능
    private RecipeData recipe;

    public event Action<ItemData, RecipeData> OnProcess;

    protected override void Awake()
    {
        base.Awake();
        if (craftButton) craftButton.onClick.AddListener(OnClickProcess);
        if (closeButton) closeButton.onClick.AddListener(Close);
    }

    public void OpenWith(InventorySlotData slot)
    {
        inputSlot.Bind(slot); // manager가 가진 슬롯 그대로 사용
        inputItem = slot.itemData;

        recipe = slot.itemData != null
            ? TestManager.Instance.inventoryManager.CraftSystem().GetTransformRecipe(slot.itemData.id)
            : null;

        if (inputNameText) inputNameText.text = inputItem?.name ?? "";

        if (recipe != null)
        {
            outputItem = TestManager.Instance.itemDatabase.GetItemById(recipe.outputItemId);
            outputSlot.Bind(new InventorySlotData(outputItem, 1)); // 출력만 새로 생성
            outputSlot.gameObject.SetActive(true);
            if (outputNameText) outputNameText.text = outputItem?.name ?? "가공 불가";
        }
        else
        {
            outputSlot.gameObject.SetActive(false);
            if (outputNameText) outputNameText.text = "가공 불가";
        }

        craftButton.interactable = (recipe != null);

        Open();
    }

    //헷갈려서 임시 주석 처리
    //public void OpenWith(ItemData input, RecipeData rcp)
    //{
    //    inputItem = input;
    //    recipe = rcp;

    //    // 입력 슬롯 미리보기
    //    if (inputSlot)
    //    {
    //        inputSlot.gameObject.SetActive(true);
    //        inputSlot.Bind(new InventorySlotData(inputItem, 1));
    //    }
    //    if (inputNameText) inputNameText.text = inputItem != null ? inputItem.name : "";

    //    // 출력 슬롯 미리보기 (레시피 없으면 안전 처리)
    //    outputItem = null;
    //    if (outputSlot)
    //    {
    //        if (recipe != null)
    //        {
    //            outputItem = TestManager.Instance.itemDatabase.GetItemById(recipe.outputItemId);
    //            outputSlot.gameObject.SetActive(true);
    //            inputSlot.Bind(new InventorySlotData(inputItem, 1));
    //        }
    //        else
    //        {
    //            outputSlot.gameObject.SetActive(false);
    //        }
    //    }
    //    if (outputNameText) outputNameText.text = (outputItem != null) ? outputItem.name : "가공 불가";

    //    // 버튼/도움말
    //    if (craftButton) craftButton.interactable = (recipe != null);

    //    Open();
    //}

    private void OnClickProcess()
    {
        if (inputItem == null || recipe == null) return;
        OnProcess?.Invoke(inputItem, recipe);
    }
}
