using System;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryEventType
{
    InventoryChanged,
    ItemDropped,
    //ItemEquipped,
    ItemUseRequested
}
public interface IInventoryMediator
{
    void Notify(object sender, InventoryEventType eventType, object data = null);
}

public class InventoryManager : MonoBehaviour
{
    private static InventoryModel model;    //solo player
    private IInventoryMediator mediator;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private void Awake()
    {
        model = new InventoryModel();
    }
    
    public void SetMediator(IInventoryMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddItem(int itemId)
    {
        model.AddItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, model.GetAllIds());
    }
    public void RemoveItem(int itemId)
    {
        model.RemoveItem(itemId);
        mediator?.Notify(this, InventoryEventType.InventoryChanged, model.GetAllIds());
    }
    public void DropItem(int itemId)
    {
        if(model.GetAmountById(itemId) > 0)
        {
            RemoveItem(itemId);
            mediator?.Notify(this, InventoryEventType.ItemDropped, itemId);
            // to do: instantiate new item. player's drop position. or somewhere.
            // Instantiate(itemDatabase.GetItemById(id).inGamePrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
            Debug.Log("Dropped: " + itemId);
        }
    }

    public int GetItemAmount(int itemId) => model.GetAmountById(itemId);
    public List<int> GetAllItems() => model.GetAllIds();
}
