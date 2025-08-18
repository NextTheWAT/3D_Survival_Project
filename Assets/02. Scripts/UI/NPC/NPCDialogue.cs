using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;
    [SerializeField] DialogueRunner runner;

    public void StartDialogue()
    {
        if (runner && dialogue) runner.Run(dialogue);
    }
    // NPC�� �ٿ��� �ǰ�, �÷��̾� ��ȣ�ۿ� ��ũ��Ʈ���� ȣ���ص� OK
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<NPCDialogue>().StartDialogue();
        }
    }

}
