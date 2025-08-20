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
    }

    private void OnDisable()
    {
        ui.OnItemClicked -= HandleSelect;
        ui.OnUseClicked -= HandleUse;
        ui.OnEquipClicked -= HandleEquip;
        ui.OnUnequipClicked -= HandleUnequip;
        ui.OnCraftClicked -= HandleCraft;
        ui.OnDropClicked -= HandleDrop;
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
        CraftSystem craftSystem =  manager.CraftSystem();
        RecipeData recipeData = craftSystem.GetTransformRecipe(selectedId.Value);
        //현재 단일 아이템 선택 시 진행하는 중..
        if (selectedId != null && recipeData != null)
        {
            if (recipeData != null)
            {
                // 제작 코루틴 실행
                StartCoroutine(craftSystem.CraftCoroutine(recipeData));
            }
        }
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
}
