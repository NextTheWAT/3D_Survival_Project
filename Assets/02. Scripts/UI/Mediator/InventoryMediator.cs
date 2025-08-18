using System.Linq;
using UnityEngine;

/// UI ↔ Model(그리고 Player 규칙) 사이를 가볍게 중재
public class InventoryMediator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private InventoryUI ui;          // View
    //[SerializeField] private InventoryModel model;    // Model
    //[SerializeField] private PlayerInventory player;  // 플레이어 규칙(사용/장착)

    private int? selectedId;

    //private void Awake()
    //{
    //    // BaseUI 라이프사이클에 맞춰 열릴 때/닫힐 때 처리하고 싶다면:
    //    ui.OnOpened.AddListener(Open);
    //    ui.OnClosed.AddListener(Close);
    //}

    //private void OnEnable()
    //{
    //    // UI 입력 이벤트
    //    ui.OnItemClicked += HandleSelect;
    //    ui.OnUseClicked += HandleUse;
    //    ui.OnEquipClicked += HandleEquip;
    //    ui.OnUnEquipClicked += HandleUnEquip;
    //    ui.OnDropClicked += HandleDrop;

    //    // Model 변경 → 리스트 갱신
    //    model.Changed += RefreshList;
    //}

    //private void OnDisable()
    //{
    //    ui.OnItemClicked -= HandleSelect;
    //    ui.OnUseClicked -= HandleUse;
    //    ui.OnEquipClicked -= HandleEquip;
    //    ui.OnUnEquipClicked -= HandleUnEquip;
    //    ui.OnDropClicked -= HandleDrop;

    //    model.Changed -= RefreshList;
    //}

    //// ===== 라이프사이클 =====
    //public void Open()
    //{
    //    selectedId = null;
    //    ui.ClearSelection();
    //    ui.SetButtonsActive(false, false, false, false);
    //    RefreshList();
    //}
    //public void Close() { /* 필요 시 정리 */ }

    //// ===== 화면 갱신 =====
    //private void RefreshList()
    //{
    //    var items = model.GetAllItemIds()
    //                     .Distinct()
    //                     .Select(id => (id,
    //                                    model.GetItemById(id).displayName,
    //                                    model.GetAmountById(id)));
    //    ui.RenderList(items); // UI가 스크롤뷰/아이템버튼 렌더링
    //}

    //// ===== 입력 처리 =====
    //private void HandleSelect(int id)
    //{
    //    selectedId = id;
    //    model.selectedItem = id;

    //    var data = model.GetItemById(id);
    //    ui.SetItemDetail(data.displayName, data.description, data.statName, data.statValueText);

    //    ui.SetButtonsActive(
    //        player.CanUse(data),
    //        player.CanEquip(data),
    //        player.CanUnEquip(data),
    //        model.GetAmountById(id) > 0
    //    );
    //}

    //private void HandleUse()
    //{
    //    if (selectedId == null) return;
    //    var data = model.GetItemById(selectedId.Value);
    //    if (!player.CanUse(data)) return;

    //    player.Use(data);
    //    model.RemoveItem(selectedId.Value); // 1개 소모
    //    PostActionRefresh();
    //}

    //private void HandleEquip()
    //{
    //    if (selectedId == null) return;
    //    var data = model.GetItemById(selectedId.Value);
    //    if (!player.CanEquip(data)) return;

    //    player.Equip(data);
    //    PostActionRefresh();
    //}

    //private void HandleUnEquip()
    //{
    //    if (selectedId == null) return;
    //    var data = model.GetItemById(selectedId.Value);
    //    if (!player.CanUnEquip(data)) return;

    //    player.UnEquip(data);
    //    PostActionRefresh();
    //}

    //private void HandleDrop()
    //{
    //    if (selectedId == null) return;
    //    model.DropItem(selectedId.Value); // 전량 버림
    //    PostActionRefresh();
    //    selectedId = null;
    //    ui.ClearSelection();
    //    ui.SetButtonsActive(false, false, false, false);
    //}

    //private void PostActionRefresh()
    //{
    //    RefreshList();
    //    if (selectedId != null && model.GetAmountById(selectedId.Value) > 0)
    //    {
    //        // 상세/버튼 상태 재계산
    //        HandleSelect(selectedId.Value);
    //    }
    //}
}
