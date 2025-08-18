using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] DialogueViewUI view;

    DialogueSO so;
    DialogueSO.Node cur;

    public void Start(DialogueSO data)
    {
        so = data;
        view.Show();

        // 버튼 바인딩(인스펙터 대신 코드로 한 번만 연결)
        view.BindNext(Next);
        view.BindClose(Stop);
        for (int i = 0; i < 10; i++) view.BindChoice(i, MakeSelect(i)); // 충분히 큰 상한

        MoveTo(so.startNode);
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
        if (cur.choices != null && cur.choices.Length > 0) return; // 선택 대기중

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

    // --- 내부 ---
    void MoveTo(string id)
    {
        cur = so.GetNode(id);
        if (cur == null) { Debug.LogWarning($"Node not found: {id}"); Stop(); return; }

        view.SetLine(cur.speakerName, cur.text);

        if (cur.choices != null && cur.choices.Length > 0)
        {
            var labels = new string[cur.choices.Length];
            for (int i = 0; i < labels.Length; i++) labels[i] = cur.choices[i].label;
            view.ShowChoices(labels);
        }
        else
        {
            view.ClearChoices();
        }
    }

    // 클로저 헬퍼: 버튼 i에 연결
    UnityEngine.Events.UnityAction MakeSelect(int i)
    {
        return () => Select(i);
    }
}
