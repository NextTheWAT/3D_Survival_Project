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
        public Choice[] choices;  // 없으면 Next 사용
        public string next;       // choices가 없을 때 다음 노드
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
