using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;
    [SerializeField] DialogueRunner runner;

    public DialogueSO Dialogue => dialogue;

    private void Awake()
    {
        runner = FindObjectOfType<DialogueRunner>();
    }

    public void StartDialogue()
    {
        if (runner && dialogue) runner.Run(dialogue);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            StartDialogue();
    }
}
