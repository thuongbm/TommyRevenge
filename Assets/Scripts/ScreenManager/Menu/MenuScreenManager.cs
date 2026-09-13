using System.Collections.Generic;
using UnityEngine;

public class MenuScreenManager : MonoBehaviour
{
    public GameObject chapterScreen;
    public GameObject chapterButton;
    public Transform chapterGridContainer;
    public ChapterScreenManager chapterScreenManager;

    [Header("Data")]
    [SerializeField] private List<ChapterSO> chapters;

    public void PopulateChapter()
    {
        foreach (Transform child in chapterGridContainer)
        {
            Destroy(child.gameObject);
        }

        if (chapters == null || chapters.Count == 0)
        {
            Debug.LogWarning("Chapters list is empty in MenuScreenManager!");
            return;
        }

        for (int i = 0; i < chapters.Count; i++)
        {
            ChapterSO currentChapter = chapters[i];
            GameObject obj = Instantiate(chapterButton, chapterGridContainer);
            ChapterButtonUI buttonUI = obj.GetComponent<ChapterButtonUI>();

            if (buttonUI != null)
            {
                buttonUI.SetUp(currentChapter, i == 0, OnChapterSelected);
            }
        }
    }

    private void OnChapterSelected(ChapterSO chosenChapter)
    {
        Debug.Log("Selected Chapter: " + chosenChapter.name);
        chapterScreenManager.OpenLevelScreen(chosenChapter);
    }

    public void OpenChapterScreen()
    {
        chapterScreen.SetActive(true);
        PopulateChapter();
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}