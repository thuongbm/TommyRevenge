using UnityEngine;
using System;


public class LevelManager : MonoBehaviour
{
    public static int totalEnemies;
    public static int enemyKilled = 0;
    public static event Action OnLevelWon;

    [Tooltip("Score points awarded per second under par time")]
    public const int PointsPerSecondUnderPar = 10;

    public static float parTime;
    public static float levelStartTime;
    public static bool timerStarted = false;

    public static int lastSpeedBonus;
    public static float lastCompletionTime;

    public static void TotalEnemies(int totalEnemiesLevelSO, float levelParTime = 60f)
    {
        totalEnemies = totalEnemiesLevelSO;
        enemyKilled = 0;
        parTime = levelParTime;
        timerStarted = false;

        ScoreManager.ResetScore();
        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.ResetCombo();
        }

        Debug.Log(totalEnemies);
    }

    public static void StartTimerIfNeeded()
    {
        if (timerStarted) return;
        timerStarted = true;
        levelStartTime = Time.time;
    }

    public static void OnEnemyKilled()
    {
        enemyKilled++;
        LevelStatus.enemyKilled = enemyKilled;

        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.RegisterKill();
        }

        Debug.Log(enemyKilled);

        if (enemyKilled >= totalEnemies)
        {
            float elapsed = timerStarted ? Time.time - levelStartTime : 0f;
            int speedBonus = Mathf.Max(0, Mathf.RoundToInt((parTime - elapsed) * PointsPerSecondUnderPar));

            ScoreManager.AddScore(speedBonus);
            lastSpeedBonus = speedBonus;
            lastCompletionTime = elapsed;

            OnLevelWon?.Invoke();
        }
    }
}
