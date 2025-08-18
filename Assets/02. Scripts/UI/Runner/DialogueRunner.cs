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

        // ��ư ���ε�(�ν����� ��� �ڵ�� �� ���� ����)
        view.BindNext(Next);
        view.BindClose(Stop);
        for (int i = 0; i < 10; i++) view.BindChoice(i, MakeSelect(i)); // ����� ū ����

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
        if (cur.choices != null && cur.choices.Length > 0) return; // ���� �����

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

    // --- ���� ---
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

    // Ŭ���� ����: ��ư i�� ����
    UnityEngine.Events.UnityAction MakeSelect(int i)
    {
        return () => Select(i);
    }
}
