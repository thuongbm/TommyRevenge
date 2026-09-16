using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private Behaviour[] gameplayBehaviours;

     private void Start()
    {
        LockGameplay();

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            UnlockGameplay
        );
    }

    private void LockGameplay()
    {
        foreach (Behaviour gameplayBehaviour in gameplayBehaviours)
        {
            if (gameplayBehaviour != null)
            {
                gameplayBehaviour.enabled = false;
            }
        }
    }

    private void UnlockGameplay()
    {
        foreach (Behaviour gameplayBehaviour in gameplayBehaviours)
        {
            if (gameplayBehaviour != null)
            {
                gameplayBehaviour.enabled = true;
            }
        }
    }
}
