using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text lableText;
    [SerializeField] private Button button;

    public void SetUp(LevelSO levelSO, bool isUnlocked)
    {
        lableText.text = levelSO.name;
        button.interactable = isUnlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            LevelStatus.currentLevel = levelSO;
            LevelStatus.enemyTotal = levelSO.enemyTotal;
            LevelStatus.ResetRun();

            LevelManager.TotalEnemies(levelSO.enemyTotal);

            SceneManager.LoadScene(levelSO.sceneToLoad);
        });
    } 
}