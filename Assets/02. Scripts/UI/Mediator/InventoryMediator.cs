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

    private int? selectedSlotId;

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

    private void HandleSelect(int slotId)
    {
        selectedSlotId = slotId;
        var slot = manager.GetSlotDatas().Find(s => s.slotId == slotId);
        if (slot != null)
            ui.BindItem(slot.itemData, slotId);

        // SetButtonsActiveByItem 등 UI 버튼 활성화도 slot 기반으로 처리
        ui.SetButtonsActiveByItem(slot?.itemData, slotId);
    }

    private void HandleUse()
    {
        if (selectedSlotId != null)
        {
            manager.UseItem(selectedSlotId.Value);
            selectedSlotId = null;
            RefreshUI();
        }
        else
        {
            Debug.Log("No item selected");
        }
    }
    private void HandleEquip()
    {
        if (selectedSlotId != null)
        {
            manager.EquipItem(selectedSlotId.Value);
            RefreshUI();
        }
    }
    private void HandleUnequip()
    {
        if (selectedSlotId != null)
        {
            manager.UnequipItem(selectedSlotId.Value);
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

        if (selectedSlotId == null) return;

        CraftSystem craftSystem = manager.CraftSystem();
        var slot = manager.GetSlotDatas().Find(s => s.slotId == selectedSlotId.Value);
        if (slot == null) return;

        RecipeData recipeData = craftSystem.GetTransformRecipe(slot.itemData.id); // itemId 기준 레시피 조회
        if (recipeData != null)
        {
            StartCoroutine(craftSystem.CraftCoroutine(recipeData));
        }
    }

    private void HandleDrop()
    {
        if (selectedSlotId != null)
        {
            manager.DropItem(selectedSlotId.Value);
            selectedSlotId = null;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (selectedSlotId != null)
        {
            var slot = manager.GetSlotDatas().Find(s => s.slotId == selectedSlotId.Value);
            if (slot != null && slot.count > 0)
            {
                HandleSelect(selectedSlotId.Value);
                return;
            }
        }

        ui.ClearSelection();
        selectedSlotId = null;
    }
}
