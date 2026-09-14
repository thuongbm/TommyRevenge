using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger Instance { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public DialogueData dialogueData;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueData);
    }
}
