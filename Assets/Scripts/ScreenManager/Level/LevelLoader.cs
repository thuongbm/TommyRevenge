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

    // Fade to black, then load a scene (level select -> gameplay, retry, back to menu...).
    // beforeLoad (optional) runs while the screen is black, right before the scene loads.
    public static void LoadTheLevel(string sceneToLoad, Action beforeLoad = null)
    {
        if (Instance == null)
        {
            Time.timeScale = 1f;
            beforeLoad?.Invoke();
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        if (Instance.isLoading) return; // ignore double clicks
        Instance.StartCoroutine(Instance.LevelTransition(sceneToLoad, beforeLoad));
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

    IEnumerator LevelTransition(string sceneToLoad, Action beforeLoad)
    {
        isLoading = true;

        animator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);

        Time.timeScale = 1f;            // the win/lose panels freeze the game; unfreeze only now
        beforeLoad?.Invoke();
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
