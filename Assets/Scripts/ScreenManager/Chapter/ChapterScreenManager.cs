using System.Collections.Generic;
using UnityEngine;

public class ChapterScreenManager : MonoBehaviour
{
    public GameObject levelScreen;
    public GameObject levelButton;
    public MenuScreenManager menuScreenManager;
    public Transform levelGridContainer;

    // Called when a chapter button is pressed
    public void OpenLevelScreen(ChapterSO selectedChapter)
    {
        levelScreen.SetActive(true);
        gameObject.SetActive(false);

        PopulateLevel(selectedChapter.levels);
    }

    public void PopulateLevel(List<LevelSO> levels)
    {
        foreach (Transform child in levelGridContainer)
        {
            Destroy(child.gameObject);
        }

        if (levels == null || levels.Count == 0)
        {
            Debug.LogWarning("No levels found in this chapter!");
            return;
        }

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject obj = Instantiate(levelButton, levelGridContainer);
            LevelButtonUI buttonUI = obj.GetComponent<LevelButtonUI>();

            if (buttonUI != null)
            {
                buttonUI.SetUp(levels[i], i == 0);
                // buttonUI.SetUp(levels[i], i == 1);
            }
        }
    }

    public void BackToChapterScreen()
    {
        levelScreen.SetActive(false);
        gameObject.SetActive(true);
    }
}