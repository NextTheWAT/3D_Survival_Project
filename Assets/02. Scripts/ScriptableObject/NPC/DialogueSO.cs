using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Game/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string startNode;
    public Node[] nodes;

    [Serializable]
    public class Node
    {
        public string id;
        public string speakerName;
        [TextArea] public string text;
        public Choice[] choices;  // ������ Next ���
        public string next;       // choices�� ���� �� ���� ���
    }

    [Serializable]
    public class Choice
    {
        public string label;
        public string next;
    }

    public Node GetNode(string id)
    {
        foreach (var n in nodes) if (n.id == id) return n;
        return null;
    }
}
