using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    public Animator animator;
    public float transitionTime = 1f;

    private bool isLoading;

    void Awake()
    {
        Instance = this;
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public static void LoadTheLevel(string sceneToLoad)
    {
        // No loader in this scene -> just load without the effect
        if (Instance == null)
        {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        if (Instance.isLoading) return; // ignore double clicks
        Instance.StartCoroutine(Instance.LevelTransition(sceneToLoad));
    }

    IEnumerator LevelTransition(string sceneToLoad)
    {
        isLoading = true;

        // Plays the fade-to-black ONLY now (when a level is chosen)
        animator.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}
