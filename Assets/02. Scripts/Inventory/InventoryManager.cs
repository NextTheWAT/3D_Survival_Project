using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;


[Serializable]
public class InventorySlotData
{
    private static int nextId = 1;
    public int slotId;
    public ItemData itemData;
    public int count;

    public InventorySlotData(ItemData itemData, int count)
    {
        this.slotId = nextId++;
        this.itemData = itemData;
        this.count = count;
    }
    public static void InitId()
    {
        nextId = 1;
    }
}
public class InventoryManager : MonoBehaviour
{
    private InventoryModel inventoryModel = new InventoryModel();
    private EquipmentModel equipmentModel = new EquipmentModel();    //solo player
    private CraftSystem craftSystem;

    private IInventoryMediator mediator;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        craftSystem = new CraftSystem(this);

        //testcode
        inventoryModel.AddItem(itemDatabase.GetItemById(1), 10);
        inventoryModel.AddItem(itemDatabase.GetItemById(2), 1);
        inventoryModel.AddItem(itemDatabase.GetItemById(3), 1);
        inventoryModel.AddItem(itemDatabase.GetItemById(4), 2);
    }
    
    public void SetMediator(IInventoryMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddItem(ItemData itemData, int amount = 1)
    {
        inventoryModel.AddItem(itemData, amount);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void RemoveOneItemFromSlot(int slotId)
    {
        inventoryModel.RemoveOneItemFromSlot(slotId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, GetSlotDatas());
    }
    public void DropItem(int slotId)
    {
        var slot = inventoryModel.GetAllSlots().Find(s => s.slotId == slotId);
        if (slot == null) return;

        if (equipmentModel.IsEquippedBySlotId(slot.slotId))
            equipmentModel.UnequipItem(slot.itemData as EquipItemData, slotId);

        RemoveOneItemFromSlot(slotId);

        //to do: 실제 드랍 오브젝트 생성
        Debug.Log($"Dropped 1 {slot.itemData.name} from slot {slotId}");
    }
    public void UseItem(int slotId)
    {
        var slot = inventoryModel.GetAllSlots().Find(s => s.slotId == slotId);
        if (slot == null) return;

        RemoveOneItemFromSlot(slotId);
        Debug.Log($"Used 1 {slot.itemData.name} from slot {slotId}");
    }

    public void EquipItem(int slotId)
    {
        var slot = inventoryModel.GetAllSlots().Find(s => s.slotId == slotId);
        if (slot == null) return;

        if (slot.itemData is EquipItemData equipItemData)
        {
            equipmentModel.EquipItem(equipItemData, slotId); // slotId 기반 장착
        }
    }
    public void UnequipItem(int slotId)
    {
        var slot = inventoryModel.GetAllSlots().Find(s => s.slotId == slotId);
        if (slot == null) return;

        if (slot.itemData is EquipItemData equipItemData)
        {
            equipmentModel.UnequipItem(equipItemData, slotId);
        }
    }

    //Test Method
    //public void PrintEquippedItems()
    //{
    //    Debug.Log("--Current equipment List--");
    //    foreach (var item in equipmentModel.GetEquippedItems())
    //    {
    //        Debug.Log(item.Value.name);
    //    }
    //}
    public int GetItemAmount(int itemId)
    {
        return inventoryModel.GetAllSlots()
                             .Where(s => s.itemData.id == itemId)
                             .Sum(s => s.count);
    }
    //public List<int> GetAllItemIds() => inventoryModel.GetAllIds();
    //public ItemData GetItemDataById(int itemId) => itemDatabase.GetItemById(itemId);
    //public List<ItemData> GetAllItemData()
    //{
    //    return inventoryModel.GetAllIds()
    //                .Select(id => itemDatabase.GetItemById(id))
    //                .ToList();
    //}
    public List<InventorySlotData> GetSlotDatas()
    {
        // slotId 포함된 슬롯 리스트 반환
        return inventoryModel.GetAllSlots();
    }

    public bool IsEquippedBySlotId(int slotId)
    {
        return equipmentModel.IsEquippedBySlotId(slotId);
    }

    public bool CanCraft(List<int> items)
    {
        return craftSystem.CanCraft(items);
    }
    public void CraftFinished(RecipeData recipe)
    {
        AddItem(itemDatabase.GetItemById(recipe.outputItemId));
    }
    public CraftSystem CraftSystem()    //임시 접근
    {
        return craftSystem;
    }
}
