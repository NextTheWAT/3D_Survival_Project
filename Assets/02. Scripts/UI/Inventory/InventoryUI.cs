using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    // ���õ� ������ ���� ǥ�ÿ� UI ���
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    // ������ ���� ��ư
    [SerializeField] private Button useButton;
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Button dropButton;

    protected override void Awake()
    {
        base.Awake();

        // ��ư �̺�Ʈ ���� (����� Debug.Log�� ���)
        if (useButton) useButton.onClick.AddListener(() => Debug.Log("Use"));
        if (EquipButton) EquipButton.onClick.AddListener(() => Debug.Log("Equip"));
        if (unEquipButton) unEquipButton.onClick.AddListener(() => Debug.Log("UnEquip"));
        if (dropButton) dropButton.onClick.AddListener(() => Debug.Log("Drop"));

        // ó�� ������ �� ��ư/�ؽ�Ʈ ��Ȱ��ȭ
        UnActive();
    }

    // ������ �� �ؽ�Ʈ, ��ư ��Ȱ��ȭ ó��
    public override void UnActive()
    {
        base.UnActive();

        // ���� ������ �ؽ�Ʈ �ʱ�ȭ
        if (selectedItemName) selectedItemName.text = string.Empty;
        if (selectedItemDescription) selectedItemDescription.text = string.Empty;
        if (selectedItemStatName) selectedItemStatName.text = string.Empty;
        if (selectedItemStatValue) selectedItemStatValue.text = string.Empty;

        // ��ư �����
        useButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    // �ܺο��� ������ ���� ���ε�
    public void BindItem(string name, string desc)
    {
        if (selectedItemName) selectedItemName.text = name;
        if (selectedItemDescription) selectedItemDescription.text = desc;
        if (selectedItemStatName) selectedItemStatName.text = "Stat Name";   // TODO: ���� ���� �̸� ���ε�
        if (selectedItemStatValue) selectedItemStatValue.text = "Stat Value"; // TODO: ���� ���� �� ���ε�
    }
}
