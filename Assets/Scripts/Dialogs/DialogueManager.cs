using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private GameObject dialogueScreen;

    private Queue<string> currentSentences; 
    private Queue<DialogueLine> dialogueBlocks;
    private Action onDialogueComplete;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dialogueBlocks = new Queue<DialogueLine>();
        currentSentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogueData, Action onComplete = null)
    {
        if (dialogueData == null || dialogueData.lines.Length == 0)
        {
            onComplete?.Invoke();
            return;
        }

        onDialogueComplete = onComplete;
        dialogueScreen.SetActive(true);
        dialogueBlocks.Clear();
        currentSentences.Clear();

        foreach (DialogueLine line in dialogueData.lines)
        {
            dialogueBlocks.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentSentences.Count == 0)
        {
            if (dialogueBlocks.Count == 0)
            {
                EndDialogue();
                return; 
            }

            DialogueLine currentSpeaker = dialogueBlocks.Dequeue();
            nameText.text = currentSpeaker.name;

            foreach (string sentence in currentSpeaker.sentences)
            {
                currentSentences.Enqueue(sentence);
            }
        }

        if (currentSentences.Count > 0)
        {
            sentenceText.text = currentSentences.Dequeue();
        }
    }

    public void EndDialogue()
    {
        dialogueScreen.SetActive(false);
        
        onDialogueComplete?.Invoke();
        onDialogueComplete = null;
    }
}