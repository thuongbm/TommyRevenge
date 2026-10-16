using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;

    [Tooltip("Seconds to wait after the last kill before the panel appears (lets the death animation play). 0 = instant")]
    [SerializeField] private float showDelay = 0.8f;

    [Header("Rank")]
    [SerializeField] private TMPro.TextMeshProUGUI rankText;
    [Tooltip("Level data whose rank thresholds are used. Empty = the level picked from the menu")]
    [SerializeField] private LevelSO levelData;

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
        ShowRank();

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

    private void ShowRank()
    {
        if (rankText == null) return;

        var level = levelData != null ? levelData : LevelStatus.currentLevel;
        string rank = LevelRank.Evaluate(ScoreManager.TotalScore, level);
        string hex = ColorUtility.ToHtmlStringRGB(LevelRank.ColorFor(rank));
        rankText.text = "RANK  <size=150%><color=#" + hex + ">" + rank + "</color></size>";

        StartCoroutine(PopRank(rankText.rectTransform));
    }

    // The game is frozen (timeScale 0) when the panel opens, so animate with unscaled time
    private System.Collections.IEnumerator PopRank(RectTransform rt)
    {
        const float duration = 0.45f;
        const float startScale = 2.5f;
        float t = 0f;
        rt.localScale = Vector3.one * startScale;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            float ease = 1f - (1f - k) * (1f - k);
            rt.localScale = Vector3.one * Mathf.Lerp(startScale, 1f, ease);
            yield return null;
        }
        rt.localScale = Vector3.one;
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
