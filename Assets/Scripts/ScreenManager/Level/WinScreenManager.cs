using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

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
