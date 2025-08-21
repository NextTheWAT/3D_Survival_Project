using UnityEngine;

[DisallowMultipleComponent]
public class EquipmentController : MonoBehaviour
{
    [Header("Where to parent the equipped item")]
    [SerializeField] private Transform grabPoint;

    private GameObject currentItem;
    private Animator currentItemAnimator;

    public Transform GrabPoint => grabPoint;
    public GameObject CurrentItem => currentItem;
    public Animator CurrentItemAnimator => currentItemAnimator;
    public bool HasItem => currentItem != null;

    /// <summary>
    /// 현재 장착 아이템을 해제하고, 새 프리팹을 GrabPoint 밑에 장착.
    /// </summary>
    public void Equip(GameObject itemPrefab)
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("[EquipmentController] Equip failed: prefab is null");
            return;
        }

        Unequip();

        currentItem = Instantiate(itemPrefab, grabPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
        currentItem.transform.localScale = Vector3.one;

        // 장착 프리팹(또는 자식)에서 Animator 캐시
        currentItemAnimator = currentItem.GetComponentInChildren<Animator>();
        if (currentItemAnimator == null)
            Debug.LogWarning("[EquipmentController] Equipped item has no Animator.");
    }

    /// <summary>
    /// 현재 장착 아이템 파괴 및 캐시 해제.
    /// </summary>
    public void Unequip()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
            currentItemAnimator = null;
        }
    }
}
