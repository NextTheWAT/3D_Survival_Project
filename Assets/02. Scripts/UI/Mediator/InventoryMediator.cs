using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InventoryEventType
{
    InventoryChanged,
    ItemDroppRequested,
    ItemEquipRequested,
    ItemUseRequested
}
public interface IInventoryMediator
{
    void Notify(object sender, InventoryEventType eventType, object data = null);
}
/// UI와 Manager를 단방향으로 중재
public class InventoryMediator : MonoBehaviour, IInventoryMediator
{
    [Header("Refs")]
    [SerializeField] private InventoryUI ui;
    [SerializeField] private InventoryManager manager;
    [SerializeField] private List<ItemData> itemDatabase;

    private int? selectedId;

    //private void Awake()
    //{
    //    // BaseUI 라이프사이클에 맞춰 열릴 때/닫힐 때 처리하고 싶다면:
    //    ui.OnOpened.AddListener(Open);
    //    ui.OnClosed.AddListener(Close);
    //}

    private void OnEnable()
    {
        // UI 입력 이벤트
        ui.OnItemClicked += HandleSelect;
        ui.OnUseClicked += HandleUse;
        ui.OnEquipClicked += HandleEquip;
        //ui.OnUnEquipClicked += HandleUnEquip;
        ui.OnDropClicked += HandleDrop;

        manager.SetMediator(this);
    }

    private void OnDisable()
    {
        ui.OnItemClicked -= HandleSelect;
        ui.OnUseClicked -= HandleUse;
        ui.OnEquipClicked -= HandleEquip;
        //ui.OnUnEquipClicked -= HandleUnEquip;
        ui.OnDropClicked -= HandleDrop;
    }

    public void Notify(object sender, InventoryEventType eventType, object data = null)
    {
        switch (eventType)
        {
            case InventoryEventType.InventoryChanged:
                RefreshList();
                break;
        }
    }

    //// ===== 라이프사이클 =====
    //public void Open()
    //{
    //    selectedId = null;
    //    ui.ClearSelection();
    //    ui.SetButtonsActive(false, false, false, false);
    //    RefreshList();
    //}
    //public void Close() { /* 필요 시 정리 */ }

    // ===== 화면 갱신 =====
    private void RefreshList()
    {
        var itemDatas = manager.GetAllItemData();
        ui.RenderList(itemDatas); // UI가 스크롤뷰/아이템버튼 렌더링
    }

    // ===== 입력 처리 =====
    private void HandleSelect(int id)
    {
        selectedId = id;
        var data = itemDatabase.FirstOrDefault(x => x.id == id);
        if (data != null) ui.BindItem(data);
        // To do
        //이거 아님. 이거 아이템 타입에 따라 자동으로 아게끔. InventoryUI 내에서도 바꾸기 SetButtonsActive 말하는 거임.
        ui.SetButtonsActive(manager.GetItemAmount(id) > 0, true, manager.GetItemAmount(id) > 0);    //Todo



        //model.selectedItem = id;

        //var data = model.GetItemById(id);
        //ui.SetItemDetail(data.displayName, data.description, data.statName, data.statValueText);

        //ui.SetButtonsActive(
        //    player.CanUse(data),
        //    player.CanEquip(data),
        //    player.CanUnEquip(data),
        //    model.GetAmountById(id) > 0
        //);
    }

    private void HandleUse()
    {
        if (selectedId != null)
        {
            manager.UseItem(selectedId.Value);
            selectedId = null;
            RefreshUI();
        }
        //if (selectedId == null) return;
        //var data = model.GetItemById(selectedId.Value);
        //if (!player.CanUse(data)) return;

        //player.Use(data);
        //model.RemoveItem(selectedId.Value); // 1개 소모
        //PostActionRefresh();
    }

    private void HandleEquip()
    {
        if (selectedId != null)
        {
            // to do: Inventory Manager에게 장착 요청하기
            Debug.Log($"Equip requested for item {selectedId.Value}");
        }
        //if (selectedId == null) return;
        //var data = model.GetItemById(selectedId.Value);
        //if (!player.CanEquip(data)) return;

        //player.Equip(data);
        //PostActionRefresh();
    }

    //private void HandleUnEquip()
    //{
    //    if (selectedId == null) return;
    //    var data = model.GetItemById(selectedId.Value);
    //    if (!player.CanUnEquip(data)) return;

    //    player.UnEquip(data);
    //    PostActionRefresh();
    //}

    private void HandleDrop()
    {
        if (selectedId != null)
        {
            manager.DropItem(selectedId.Value);
            selectedId = null;
            RefreshUI();
        }
        //if (selectedId == null) return;
        //model.DropItem(selectedId.Value); // 전량 버림
        //PostActionRefresh();
        //selectedId = null;
        //ui.ClearSelection();
        //ui.SetButtonsActive(false, false, false, false);
    }

    //private void PostActionRefresh()
    //{
    //    RefreshList();
    //    if (selectedId != null && model.GetAmountById(selectedId.Value) > 0)
    //    {
    //        // 상세/버튼 상태 재계산
    //        HandleSelect(selectedId.Value);
    //    }
    //}
    private void RefreshUI()
    {
        if (selectedId != null && manager.GetItemAmount(selectedId.Value) > 0)
            HandleSelect(selectedId.Value);
        else
            ui.ClearSelection();
    }
}
