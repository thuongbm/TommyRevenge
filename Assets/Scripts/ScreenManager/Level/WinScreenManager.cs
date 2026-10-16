using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI rankText;

    [Tooltip("Seconds to wait after the last kill before the panel appears (lets the death animation play). 0 = instant")]
    [SerializeField] private float showDelay = 0.8f;

    void OnEnable()
    {
        LevelManager.OnLevelWon += ShowWinPanel;
    }

    void OnDisable()
    {
        LevelManager.OnLevelWon -= ShowWinPanel;
    }

    private void ShowWinPanel()
    {
        StartCoroutine(ShowAfterDelay());
    }

    private System.Collections.IEnumerator ShowAfterDelay()
    {
        if (showDelay > 0f) yield return new WaitForSeconds(showDelay);
        ShowNow();
    }

    private void ShowNow()
    {
        GameFreeze.Freeze();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + ScoreManager.TotalScore;
        }

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(LevelManager.lastCompletionTime / 60f);
            int seconds = Mathf.FloorToInt(LevelManager.lastCompletionTime % 60f);
            timeText.text = string.Format("Time: {0:00}:{1:00}  (+{2} speed bonus)", minutes, seconds, LevelManager.lastSpeedBonus);
        }

        if (rankText != null)
        {
            string rank = LevelRank.Evaluate(ScoreManager.TotalScore, LevelStatus.currentLevel);
            rankText.text = rank;
            rankText.color = LevelRank.ColorFor(rank);
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        // Reset the run so kill counter / score / timer start fresh
        LevelStatus.ResetRun();
        LevelManager.TotalEnemies(LevelManager.totalEnemies, LevelManager.parTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("0");
    }
}
