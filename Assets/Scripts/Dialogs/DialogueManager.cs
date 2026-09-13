using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;

    private Queue<string> currentSentences; 
    private Queue<DialogueLine> dialogueBlocks;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        dialogueBlocks = new Queue<DialogueLine>();
        currentSentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        Debug.Log("Start conversation");
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

        string currentSentence = currentSentences.Dequeue();
        sentenceText.text = currentSentence;
    }

    public void EndDialogue()
    {
        
    }
}
