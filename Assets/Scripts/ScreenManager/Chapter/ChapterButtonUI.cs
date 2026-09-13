using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tileChapter;
    [SerializeField] private Button button;

    public void SetUp(ChapterSO chapterSO, bool isUnlocked, Action<ChapterSO> onChapterClicked)
    {
        tileChapter.text = string.IsNullOrEmpty(chapterSO.title) ? chapterSO.name : chapterSO.title;
        button.interactable = isUnlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            onChapterClicked?.Invoke(chapterSO);
        });
    }
}