using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField]
    private Transform handGrabPoint;
    private GameObject currentItem;
    private PlayerInteractionController playerInteractionController;

    public GameObject CurrentItem => currentItem;
    public Transform HandGrabPoint => handGrabPoint;

    private void Awake()
    {
        playerInteractionController = GetComponent<PlayerInteractionController>();
    }
    //장착
    public void Equip(EquipItemData itemData)
    {
        if (itemData == null || itemData.inGamePrefab == null) return;

        //기존 장착 아이템 제거
        if (currentItem != null)
            Destroy(currentItem);

        //grab point에 붙이기
        currentItem = Instantiate(itemData.inGamePrefab, handGrabPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        SetEquipptedItemForInteraction(itemData);
        Debug.Log($"{itemData.name} equipped");
    }

    //장착 해제
    public void Unequip()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
            SetEquipptedItemForInteraction(null);
        }
    }

    private void SetEquipptedItemForInteraction(EquipItemData? equipItemData)
    {
        playerInteractionController.SetEquipptedItem(equipItemData);
    }
}
