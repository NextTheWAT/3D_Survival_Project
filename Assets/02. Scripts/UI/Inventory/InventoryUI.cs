using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    // 선택된 아이템 정보 표시용 UI 요소
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    // 아이템 조작 버튼
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Button craftButton;
    [SerializeField] private Button dropButton;

    [Header("Slot Setting")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int maxSlotCount = 14;

    private List<InventorySlotUI> slots = new();

    public event Action<int> OnItemClicked;
    public event Action OnUseClicked;
    public event Action OnEquipClicked;
    public event Action OnUnequipClicked;
    public event Action OnCraftClicked;
    public event Action OnDropClicked;


    private int? selectedSlotId;

    protected override void Awake()
    {
        base.Awake();

        // 버튼 이벤트 연결 (현재는 Debug.Log만 출력)
        if (useButton) useButton.onClick.AddListener(() => OnUseClicked?.Invoke());
        if (equipButton) equipButton.onClick.AddListener(() => OnEquipClicked?.Invoke());
        if (unEquipButton) unEquipButton.onClick.AddListener(() => OnUnequipClicked?.Invoke());
        if (craftButton) craftButton.onClick.AddListener(() => OnCraftClicked?.Invoke());
        if (dropButton) dropButton.onClick.AddListener(() => OnDropClicked?.Invoke());

        // 처음 시작할 때 버튼/텍스트 비활성화
        UnActive();

        //슬롯 생성
        for (int i = 0; i < maxSlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();

            slotObj.SetActive(false); // 처음엔 꺼둠
            slots.Add(slotUI);
        }
    }

    // 시작할 때 텍스트, 버튼 비활성화 처리
    public override void UnActive()
    {
        base.UnActive();

        // 선택 아이템 텍스트 초기화
        if (selectedItemName) selectedItemName.text = string.Empty;
        if (selectedItemDescription) selectedItemDescription.text = string.Empty;
        if (selectedItemStatName) selectedItemStatName.text = string.Empty;
        if (selectedItemStatValue) selectedItemStatValue.text = string.Empty;

        // 버튼 숨기기
        SetDisactiveButtons();
    }
    public void Init(List<InventorySlotData> initialSlots)
    {
        RenderList(initialSlots);
        ClearSelection();
    }

    public void BindItem(ItemData data, int slotId)
    {
        if (selectedItemName) selectedItemName.text = data.displayName;
        if (selectedItemDescription) selectedItemDescription.text = data.description;
        if (selectedItemStatName) selectedItemStatName.text = "Stat Name";   // TODO: 실제 스탯 이름 바인딩
        if (selectedItemStatValue) selectedItemStatValue.text = "Stat Value"; // TODO: 실제 스탯 값 바인딩

        SetButtonsActiveByItem(data, slotId); // slotId 전달
    }

    public void RenderList(List<InventorySlotData> slotDatas)
    {
        //to do: 업데이트된 아이템 데이터 리스트로 렌더 다시하기
        foreach (var slot in slots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < slotDatas.Count && i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Bind(slotDatas[i]);

            var capturedIndex = i;
            //slots[i].onClick = () => SelectItem(slotDatas[capturedIndex].itemData);
            slots[i].onClick = () => SelectItem(slotDatas[capturedIndex].slotId);
        }
    }
    public void ClearSelection()
    {
        //to do: 선택한 아이템 없음 상태.
        selectedSlotId = null;
        if (selectedItemName) selectedItemName.text = "";
        if (selectedItemDescription) selectedItemDescription.text = "";
        if (selectedItemStatName) selectedItemStatName.text = "";
        if (selectedItemStatValue) selectedItemStatValue.text = "";
        SetDisactiveButtons();
    }
    public void SetDisactiveButtons()    //개별로 필요한 버튼 on/off하는 방식인데 case로 아이템 유형에 따라하는 것(과제처럼)이 나을지도 모르겠음.
    {
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }
    public void SetButtonsActiveByItem(ItemData itemData, int slotId)
    {
        //to do:
        //recipe에 1:1 가공 가능한 아이템이 있으면 가공버튼 활성화해야함.
        bool canCraft = false;

        // 1:1 변환 레시피가 있는지 확인
        RecipeData recipe = GameManager.Instance.inventoryManager.CraftSystem().GetTransformRecipe(itemData.id);
        if (recipe != null)
        {
            canCraft = true;
        }

        if (itemData is ConsumeItemData)  // 소모품이면
        {
            useButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(false);
            craftButton.gameObject.SetActive(canCraft);
            dropButton.gameObject.SetActive(true);
        }
        else if (itemData is EquipItemData equipItemData)  // 장비면
        {
            useButton.gameObject.SetActive(false);
            bool isEquipped = GameManager.Instance.inventoryManager.IsEquippedBySlotId(slotId);
            equipButton.gameObject.SetActive(!isEquipped);
            unEquipButton.gameObject.SetActive(isEquipped);
            craftButton.gameObject.SetActive(canCraft); //??z
            dropButton.gameObject.SetActive(true);
        }
        else  // 그 외 아이템
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(false);
            craftButton.gameObject.SetActive(canCraft);
            dropButton.gameObject.SetActive(true);
        }
    }
    public void SelectItem(int slotId)
    {
        selectedSlotId = slotId;
        var slotData = GameManager.Instance.inventoryManager.GetSlotDatas()
                         .Find(s => s.slotId == slotId);
        if (slotData != null)
        {
            BindItem(slotData.itemData, slotId);
            OnItemClicked?.Invoke(slotId);
        }
    }
}
