using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueViewUI : BaseUI
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI speaker;
    [SerializeField] TextMeshProUGUI line;

    [Header("Buttons")]
    [SerializeField] Button nextButton;
    [SerializeField] Button closeButton;

    [Header("Choices (���� ����)")]
    [SerializeField] Button[] choiceButtons;          // �̸� ���� ��ư��
    [SerializeField] TextMeshProUGUI[] choiceLabels;  // �� ��ư�� ��

    public void BindNext(UnityEngine.Events.UnityAction action)
    {
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(action);
    }
    public void BindClose(UnityEngine.Events.UnityAction action)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(action);
    }
    public void BindChoice(int i, UnityEngine.Events.UnityAction action)
    {
        if (i < 0 || i >= choiceButtons.Length) return;
        choiceButtons[i].onClick.RemoveAllListeners();
        choiceButtons[i].onClick.AddListener(action);
    }

    public void SetLine(string s, string t)
    {
        if (speaker) speaker.text = s;
        if (line) line.text = t;
    }

    public void ShowChoices(string[] labels)
    {
        // Next �����, ������ Ȱ��ȭ
        if (nextButton) nextButton.gameObject.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            bool on = i < labels.Length;
            choiceButtons[i].gameObject.SetActive(on);
            if (on && i < choiceLabels.Length) choiceLabels[i].text = labels[i];
        }
    }

    public void ClearChoices()
    {
        // Next ���̱�, ������ ����
        if (nextButton) nextButton.gameObject.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
            choiceButtons[i].gameObject.SetActive(false);
    }

    // BaseUI ����
    public void Show() => Open();
    public void Hide() => Close();
}
