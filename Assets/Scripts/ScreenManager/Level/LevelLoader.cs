using System;
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

    // Fade to black, then load a scene (level select -> gameplay)
    public static void LoadTheLevel(string sceneToLoad)
    {
        if (Instance == null)
        {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        if (Instance.isLoading) return; // ignore double clicks
        Instance.StartCoroutine(Instance.LevelTransition(sceneToLoad));
    }

    // Fade to black, run your code while the screen is covered, then fade back in.
    // Use this for ANY screen/panel switch inside the same scene
    // (menu -> chapters, chapters -> levels, back buttons...).
    public static void PlayTransition(Action onScreenCovered)
    {
        if (Instance == null)
        {
            onScreenCovered?.Invoke();
            return;
        }

        if (Instance.isLoading) return;
        Instance.StartCoroutine(Instance.PanelTransition(onScreenCovered));
    }

    IEnumerator LevelTransition(string sceneToLoad)
    {
        isLoading = true;

        animator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator PanelTransition(Action onScreenCovered)
    {
        isLoading = true;

        animator.SetTrigger("Start");                 // fade to black
        yield return new WaitForSecondsRealtime(transitionTime);

        onScreenCovered?.Invoke();                      // swap the screens while it's black

        animator.SetTrigger("End");                   // fade back in
        yield return new WaitForSecondsRealtime(transitionTime);

        isLoading = false;                              // allow the next click
    }
}
