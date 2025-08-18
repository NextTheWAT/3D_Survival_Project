using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;
    [SerializeField] DialogueRunner runner;

    public void StartDialogue()
    {
        if (runner && dialogue) runner.Run(dialogue);
    }
    // NPC에 붙여도 되고, 플레이어 상호작용 스크립트에서 호출해도 OK
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<NPCDialogue>().StartDialogue();
        }
    }

}
