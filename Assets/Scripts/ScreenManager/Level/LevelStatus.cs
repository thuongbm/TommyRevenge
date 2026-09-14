using UnityEngine;

public static class LevelStatus
{
    public static LevelSO currentLevel { get; set; }
    public static int enemyTotal { get; set; }
    public static int enemyKilled { get; set; }

    public static void ResetRun()
    {
        Debug.Log("Reset");
        enemyKilled = 0;
    }
}