using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class NPCNameTag : MonoBehaviour
{
    [SerializeField] NPCDialogue npcDialogue;        // 같은 오브젝트에 붙어있다면 자동 할당 가능
    [SerializeField] TextMeshProUGUI nameText;       // World Space TMP
    [SerializeField] Vector3 offset = new Vector3(0f, 2.2f, 0f);
    [SerializeField] bool facePlayer = true;

    private Transform player;

    void Awake()
    {
        if (!npcDialogue) npcDialogue = GetComponent<NPCDialogue>();
        if (!nameText) nameText = GetComponentInChildren<TextMeshProUGUI>(true);

        // 플레이어 찾기 (Tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;
    }

    void Start()
    {
        if (nameText && npcDialogue && npcDialogue.Dialogue)
        {
            nameText.text = npcDialogue.Dialogue.GetDefaultSpeaker();
            nameText.gameObject.SetActive(true);
        }
    }

    void LateUpdate()
    {
        if (!nameText) return;

        // 위치 보정
        nameText.transform.position = transform.position + offset;

        // 플레이어를 바라보게
        if (facePlayer && player)
        {
            var dir = nameText.transform.position - player.position;
            nameText.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    // 외부에서 화자 이름 갱신 가능
    public void SetSpeaker(string speaker)
    {
        if (nameText) nameText.text = string.IsNullOrEmpty(speaker) ? "NPC" : speaker;
    }
}
