using System.Linq;
using UnityEngine;

/// UI �� Model(�׸��� Player ��Ģ) ���̸� ������ ����
public class InventoryMediator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private InventoryUI ui;          // View
    //[SerializeField] private InventoryModel model;    // Model
    //[SerializeField] private PlayerInventory player;  // �÷��̾� ��Ģ(���/����)

    private int? selectedId;

    //private void Awake()
    //{
    //    // BaseUI ����������Ŭ�� ���� ���� ��/���� �� ó���ϰ� �ʹٸ�:
    //    ui.OnOpened.AddListener(Open);
    //    ui.OnClosed.AddListener(Close);
    //}

    //private void OnEnable()
    //{
    //    // UI �Է� �̺�Ʈ
    //    ui.OnItemClicked += HandleSelect;
    //    ui.OnUseClicked += HandleUse;
    //    ui.OnEquipClicked += HandleEquip;
    //    ui.OnUnEquipClicked += HandleUnEquip;
    //    ui.OnDropClicked += HandleDrop;

    //    // Model ���� �� ����Ʈ ����
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

    //// ===== ����������Ŭ =====
    //public void Open()
    //{
    //    selectedId = null;
    //    ui.ClearSelection();
    //    ui.SetButtonsActive(false, false, false, false);
    //    RefreshList();
    //}
    //public void Close() { /* �ʿ� �� ���� */ }

    //// ===== ȭ�� ���� =====
    //private void RefreshList()
    //{
    //    var items = model.GetAllItemIds()
    //                     .Distinct()
    //                     .Select(id => (id,
    //                                    model.GetItemById(id).displayName,
    //                                    model.GetAmountById(id)));
    //    ui.RenderList(items); // UI�� ��ũ�Ѻ�/�����۹�ư ������
    //}

    //// ===== �Է� ó�� =====
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
    //    model.RemoveItem(selectedId.Value); // 1�� �Ҹ�
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
    //    model.DropItem(selectedId.Value); // ���� ����
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
    //        // ��/��ư ���� ����
    //        HandleSelect(selectedId.Value);
    //    }
    //}
}
