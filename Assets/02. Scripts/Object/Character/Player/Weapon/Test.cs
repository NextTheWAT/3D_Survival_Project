
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Transform handGrabPoint;
    [Header("Where to parent the equipped item")]
    [SerializeField] private Transform grabPoint;

    private GameObject currentItem;
    private PlayerInteractionController playerInteractionController;
    private Animator currentItemAnimator;

    public Transform GrabPoint => grabPoint;
    public GameObject CurrentItem => currentItem;
    public Transform HandGrabPoint => handGrabPoint;
    public Animator CurrentItemAnimator => currentItemAnimator;
    public bool HasItem => currentItem != null;

    private void Awake()
    {
        playerInteractionController = GetComponent<PlayerInteractionController>();
    }
    //장착
    /// <summary>
    /// 현재 장착 아이템을 해제하고, 새 프리팹을 GrabPoint 밑에 장착.
    /// </summary>
    public void Equip(GameObject itemPrefab)
    {
        if (itemData == null || itemData.inGamePrefab == null) return;
        if (itemPrefab == null)
        {
            Debug.LogWarning("[EquipmentController] Equip failed: prefab is null");
            return;
        }

        //기존 장착 아이템 제거
        if (currentItem != null)
            Destroy(currentItem);
        Unequip();

        //grab point에 붙이기
        currentItem = Instantiate(itemData.inGamePrefab, handGrabPoint);
        currentItem = Instantiate(itemPrefab, grabPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
        currentItem.transform.localScale = Vector3.one;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        Collider[] colliders = currentItem.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            Destroy(col);
        }

        SetEquipptedItemForInteraction(itemData);
        Debug.Log($"{itemData.name} equipped");
        // 장착 프리팹(또는 자식)에서 Animator 캐시
        currentItemAnimator = currentItem.GetComponentInChildren<Animator>();
        if (currentItemAnimator == null)
            Debug.LogWarning("[EquipmentController] Equipped item has no Animator.");
    }

    //장착 해제
    /// <summary>
    /// 현재 장착 아이템 파괴 및 캐시 해제.
    /// </summary>
    public void Unequip()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
            SetEquipptedItemForInteraction(null);
            currentItemAnimator = null;
        }
    }

    private void SetEquipptedItemForInteraction(EquipItemData? equipItemData)
    {
        playerInteractionController.SetEquipptedItem(equipItemData);
    }
}