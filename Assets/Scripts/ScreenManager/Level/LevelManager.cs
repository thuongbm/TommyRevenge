using UnityEngine;
using System;


public class LevelManager : MonoBehaviour
{
    public static int totalEnemies;
    public static int enemyKilled = 0;
    public static event Action OnLevelWon;


public static void TotalEnemies(int totalEnemiesLevelSO)
    {
        totalEnemies = totalEnemiesLevelSO;
        enemyKilled = 0;

        Debug.Log(totalEnemies);
    }
public static void OnEnemyKilled()
    {
        enemyKilled++;
        LevelStatus.enemyKilled = enemyKilled;

        Debug.Log(enemyKilled);

        if (enemyKilled >= totalEnemies)
        {
            OnLevelWon?.Invoke();
        }
    }
    
}
