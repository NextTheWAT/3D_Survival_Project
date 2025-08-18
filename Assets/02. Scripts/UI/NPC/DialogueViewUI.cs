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

    [Header("Choices (고정 개수)")]
    [SerializeField] Button[] choiceButtons;          // 미리 만든 버튼들
    [SerializeField] TextMeshProUGUI[] choiceLabels;  // 각 버튼의 라벨

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
        // Next 숨기고, 선택지 활성화
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
        // Next 보이기, 선택지 숨김
        if (nextButton) nextButton.gameObject.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
            choiceButtons[i].gameObject.SetActive(false);
    }

    // BaseUI 래핑
    public void Show() => Open();
    public void Hide() => Close();
}
