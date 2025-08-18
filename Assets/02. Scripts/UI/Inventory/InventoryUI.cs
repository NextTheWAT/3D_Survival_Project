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
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Button dropButton;

    public event Action<int> OnItemClicked;
    public event Action OnUseClicked;
    public event Action OnEquipClicked;
    public event Action OnDropClicked;

    private int? selectedItemId;

    protected override void Awake()
    {
        base.Awake();

        // 버튼 이벤트 연결 (현재는 Debug.Log만 출력)
        if (useButton) useButton.onClick.AddListener(() => OnUseClicked?.Invoke());
        if (EquipButton) EquipButton.onClick.AddListener(() => OnEquipClicked?.Invoke());
        //if (unEquipButton) unEquipButton.onClick.AddListener(() => OnEquipClicked?.Invoke()); //일단 EquipClicked 하나로만 되는지 보고, 
        if (dropButton) dropButton.onClick.AddListener(() => OnDropClicked?.Invoke());

        // 처음 시작할 때 버튼/텍스트 비활성화
        UnActive();
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
        useButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    // 외부에서 아이템 정보 바인딩
    public void BindItem(string name, string desc)
    {
        if (selectedItemName) selectedItemName.text = name;
        if (selectedItemDescription) selectedItemDescription.text = desc;
        if (selectedItemStatName) selectedItemStatName.text = "Stat Name";   // TODO: 실제 스탯 이름 바인딩
        if (selectedItemStatValue) selectedItemStatValue.text = "Stat Value"; // TODO: 실제 스탯 값 바인딩
    }

    public void RenderList(List<ItemData> itemDatas)
    { 
        //to do: 업데이트된 아이템 데이터 리스트로 렌더 다시하기
    }
    public void ClearSelection()
    {
        //to do: 선택한 아이템 없음 상태.
    }
}
