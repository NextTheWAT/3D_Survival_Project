using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;
    [SerializeField] DialogueRunner runner;

    public void StartDialogue()
    {
        if (runner && dialogue) runner.Start(dialogue);
    }
}
