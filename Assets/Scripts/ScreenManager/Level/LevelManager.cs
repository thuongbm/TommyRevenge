using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int totalEnemies;
    public static int enemyKilled = 0;

    public static void TotalEnemies(int totalEnemiesLevelSO)
    {
        totalEnemies = totalEnemiesLevelSO;

        Debug.Log(totalEnemies);
    }
    public static void OnEnemyKilled()
    {
        enemyKilled++;
        LevelStatus.enemyKilled = enemyKilled;
        
        Debug.Log(enemyKilled);

        LevelStatus.enemyKilled = enemyKilled;

        if (enemyKilled >= totalEnemies)
        {
            
        }
    }
    
}
