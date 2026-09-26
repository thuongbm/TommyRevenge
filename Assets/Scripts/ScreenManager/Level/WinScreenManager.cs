using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;

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
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("0");
    }
}
