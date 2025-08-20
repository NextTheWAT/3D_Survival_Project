// DialogueRunner.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] DialogueViewUI view;

    DialogueSO so;
    DialogueSO.Node cur;

    public void Run(DialogueSO data)
    {
        so = data;
        view.Show();

        view.BindNext(OnClickNext);
        view.BindClose(Stop);
        for (int i = 0; i < 10; i++) view.BindChoice(i, MakeSelect(i));

        // 타이핑 완료 콜백 구독
        view.OnTypingCompleted = OnTypingCompleted;

        MoveTo(so.startNode);
    }

    void OnTypingCompleted()
    {
        // 타이핑 끝났다면, 선택지가 있는 노드라면 여기서 노출
        if (cur != null && cur.choices != null && cur.choices.Length > 0)
        {
            var labels = new string[cur.choices.Length];
            for (int i = 0; i < labels.Length; i++) labels[i] = cur.choices[i].label;
            view.ShowChoices(labels);
        }
    }

    void OnClickNext()
    {
        // 타이핑 중이면 스킵, 아니면 다음으로
        if (view.IsTyping)
        {
            view.SkipTyping();
            return;
        }
        Next();
    }

    public void Stop()
    {
        view.Hide();
        so = null;
        cur = null;
    }

    public void Next()
    {
        if (cur == null) return;
        if (cur.choices != null && cur.choices.Length > 0) return;

        if (string.IsNullOrEmpty(cur.next)) { Stop(); return; }
        MoveTo(cur.next);
    }

    public void Select(int index)
    {
        if (cur == null || cur.choices == null) return;
        if (index < 0 || index >= cur.choices.Length) return;

        string next = cur.choices[index].next;
        if (string.IsNullOrEmpty(next)) { Stop(); return; }
        MoveTo(next);
    }

    void MoveTo(string id)
    {
        cur = so.GetNode(id);
        if (cur == null) { Debug.LogWarning($"Node not found: {id}"); Stop(); return; }

        // 핵심 변경: 즉시 SetLine 대신 타이핑으로 출력
        view.SetLineTyping(cur.speakerName, cur.text);

        // 선택지는 타이핑 완료 이벤트에서 노출
    }

    UnityEngine.Events.UnityAction MakeSelect(int i) => () => Select(i);
}
