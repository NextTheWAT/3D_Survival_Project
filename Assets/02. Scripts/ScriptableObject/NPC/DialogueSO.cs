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

    // 시작 노드 기준 스피커 이름
    public string GetDefaultSpeaker()
    {
        var n = GetNode(startNode);
        if (n != null && !string.IsNullOrEmpty(n.speakerName)) return n.speakerName;

        // 폴백: 첫 번째로 발견되는 유효 스피커
        foreach (var node in nodes)
            if (!string.IsNullOrEmpty(node.speakerName)) return node.speakerName;

        return "NPC";
    }
}
