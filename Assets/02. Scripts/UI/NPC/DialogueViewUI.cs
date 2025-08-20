using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

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

    Coroutine typingCo;
    string fullSpeaker, fullLine;
    int showCount;
    public bool IsTyping { get; private set; }
    public UnityAction OnTypingCompleted; // 러너가 구독

    [SerializeField] float charsPerSecond = 30f;

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

    public void SetLineTyping(string speakerStr, string lineStr)
    {
        // 버튼/선택지 잠시 비활성 or 다음 버튼 숨김
        ClearChoices();
        if (nextButton) nextButton.gameObject.SetActive(true);
        if (typingCo != null) StopCoroutine(typingCo);

        fullSpeaker = speakerStr ?? "";
        fullLine = lineStr ?? "";
        showCount = 0;
        if (speaker) speaker.text = fullSpeaker;
        if (line) line.text = "";

        typingCo = StartCoroutine(CoType());
    }

    IEnumerator CoType()
    {
        IsTyping = true;
        float delay = 1f / Mathf.Max(1f, charsPerSecond);

        while (showCount < fullLine.Length)
        {
            showCount++;
            if (line) line.text = fullLine.Substring(0, showCount);
            yield return new WaitForSecondsRealtime(delay);
        }

        IsTyping = false;
        typingCo = null;
        OnTypingCompleted?.Invoke(); // 러너에 "다 찍었어!" 알림
    }

    public void SkipTyping()
    {
        if (!IsTyping) return;
        if (typingCo != null) StopCoroutine(typingCo);
        typingCo = null;
        IsTyping = false;
        if (line) line.text = fullLine;
        OnTypingCompleted?.Invoke();
    }



}
