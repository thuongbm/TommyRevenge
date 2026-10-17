using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text lableText;
    [SerializeField] private Button button;

    public void SetUp(LevelSO levelSO, bool isUnlocked)
    {
        lableText.text = levelSO.levelName;
        button.interactable = isUnlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            LevelStatus.currentLevel = levelSO;
            LevelStatus.enemyTotal = levelSO.enemyTotal;
            LevelStatus.ResetRun();

            LevelManager.TotalEnemies(levelSO.enemyTotal, levelSO.parTime);

            LevelLoader.LoadTheLevel(levelSO.sceneToLoad);

        });
    } 
}