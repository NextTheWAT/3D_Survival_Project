using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InventoryEventType
{
    InventoryChanged,
    ItemDroppRequested,
    ItemEquipRequested,
    ItemCraftRequested,
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
    [SerializeField] private CraftUI craftUI;

    private int? selectedId;

    private void OnEnable()
    {
        // UI 입력 이벤트
        ui.OnItemClicked += HandleSelect;
        ui.OnUseClicked += HandleUse;
        ui.OnEquipClicked += HandleEquip;
        ui.OnUnequipClicked += HandleUnequip;
        ui.OnCraftClicked += HandleCraft;
        ui.OnDropClicked += HandleDrop;

        manager.SetMediator(this);

        // 초기 렌더링
        ui.Init(manager.GetSlotDatas());

        // 가공창 결과(확정) 콜백 구독
        if (craftUI) craftUI.OnProcess += HandleCraftConfirmed;
    }

    private void OnDisable()
    {
        ui.OnItemClicked -= HandleSelect;
        ui.OnUseClicked -= HandleUse;
        ui.OnEquipClicked -= HandleEquip;
        ui.OnUnequipClicked -= HandleUnequip;
        ui.OnCraftClicked -= HandleCraft;
        ui.OnDropClicked -= HandleDrop;

        // 구독 해제
        if (craftUI) craftUI.OnProcess -= HandleCraftConfirmed;
    }

    public void Notify(object sender, InventoryEventType eventType, object data = null)
    {
        switch (eventType)
        {
            case InventoryEventType.InventoryChanged:
                ui.RenderList(data as List<InventorySlotData>);
                break;
        }
    }

    private void HandleSelect(int id)
    {
        selectedId = id;

        ui.BindItem(manager.GetItemDataById(selectedId.Value));
        //var data = manager.GetItemDataById(id);
        //if (data != null) ui.SelectItem(data);
        // To do
        //이거 아님. 이거 아이템 타입에 따라 자동으로 아게끔. InventoryUI 내에서도 바꾸기 SetButtonsActive 말하는 거임.
    }

    private void HandleUse()
    {
        if (selectedId != null)
        {
            manager.UseItem(selectedId.Value);
            selectedId = null;
            RefreshUI();
        }
        else
        {
            Debug.Log("No item selected");
        }
    }

    private void HandleEquip()
    {
        if (selectedId != null)
        {
            Debug.Log($"Equip requested for item {selectedId.Value}");
            manager.EquipItem(selectedId.Value);
            RefreshUI();
        }
    }
    private void HandleUnequip()
    {
        if (selectedId != null)
        {
            // to do: Inventory Manager에게 장착 요청하기
            Debug.Log($"Unquip requested for item {selectedId.Value}");
            manager.UnequipItem(selectedId.Value);
            RefreshUI();
        }
    }

    private void HandleCraft()
    {
        if (manager.GetSlotDatas().Count >= 14)
        {
            Debug.Log("you need at least 1 empty slot in your inventory.");
            return;
        }
        if (selectedId == null) return; // 먼저 체크하고

        var craftSystem = manager.CraftSystem();
        var recipe = craftSystem.GetTransformRecipe(selectedId.Value);
        if (recipe == null) return;

        var itemData = manager.GetItemDataById(selectedId.Value);

        // 바로 코루틴 실행하지 말고 CraftUI 오픈
        if (craftUI) craftUI.OpenWith(itemData, recipe);
    }

    private void HandleDrop()
    {
        if (selectedId != null)
        {
            manager.DropItem(selectedId.Value);
            selectedId = null;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (selectedId != null && manager.GetItemAmount(selectedId.Value) > 0)
        {
            HandleSelect(selectedId.Value);
        }
        else
        {
            ui.ClearSelection();
        }
    }

    // CraftUI 확정 콜백에서 실제 제작 실행
    private void HandleCraftConfirmed(ItemData input, RecipeData recipe)
    {
        StartCoroutine(manager.CraftSystem().CraftCoroutine(recipe));
        if (craftUI) craftUI.Close();
    }

}
