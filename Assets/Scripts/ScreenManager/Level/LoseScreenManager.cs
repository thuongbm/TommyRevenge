using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Shows the "You Died" panel (Retry / Back to Menu) when the player dies.
/// Put this on an object that is ALWAYS active - the panel itself starts inactive.
/// </summary>
public class LoseScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Tooltip("Seconds to wait after dying before the panel appears")]
    [SerializeField] private float showDelay = 1f;

    [Tooltip("Freeze the game and cut the player's controls while the panel is open")]
    [SerializeField] private bool pauseOnShow = true;

    void OnEnable()
    {
        BodyMovement.OnPlayerDied += HandlePlayerDied;
    }

    void OnDisable()
    {
        BodyMovement.OnPlayerDied -= HandlePlayerDied;
    }

    private static bool LevelAlreadyWon()
    {
        return LevelManager.totalEnemies > 0 && LevelManager.enemyKilled >= LevelManager.totalEnemies;
    }

    private void HandlePlayerDied()
    {
        // Don't pop the lose screen on top of the win screen
        if (LevelAlreadyWon()) return;
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(showDelay);
        if (LevelAlreadyWon()) yield break;
        Show();
    }

    private void Show()
    {
        if (losePanel != null) losePanel.SetActive(true);
        if (scoreText != null) scoreText.text = "Score: " + ScoreManager.TotalScore;
        if (pauseOnShow) GameFreeze.Freeze();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        // Reset the run (kill counter, timer, score, combo) before reloading
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
