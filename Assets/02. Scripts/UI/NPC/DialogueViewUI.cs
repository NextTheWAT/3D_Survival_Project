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
        if (nextButton) nextButton.gameObject.SetActive(false);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            bool on = i < labels.Length;

            // 버튼 on/off
            if (choiceButtons[i]) choiceButtons[i].gameObject.SetActive(on);

            // 라벨 on/off + 텍스트
            if (i < choiceLabels.Length && choiceLabels[i])
            {
                var labelGO = choiceLabels[i].gameObject;
                if (labelGO) labelGO.SetActive(on);
                choiceLabels[i].text = on ? labels[i] : string.Empty;
            }
        }
    }

    public void ClearChoices()
    {
        if (nextButton) nextButton.gameObject.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i]) choiceButtons[i].gameObject.SetActive(false);
            if (i < choiceLabels.Length && choiceLabels[i])
            {
                choiceLabels[i].gameObject.SetActive(false);
                choiceLabels[i].text = string.Empty;
            }
        }
    }


    // BaseUI 래핑
    public void Show() => Open();
    public void Hide() => Close();
}
