using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Levels/LevelsData")]
public class LevelSO : ScriptableObject
{
    [Tooltip("Display name shown on the level select button")]
    public string levelName;
    public string sceneToLoad;
    public int enemyTotal;
    public float parTime = 60f;

    public bool isUnlockedByDefault;

    [Header("Rank Thresholds (minimum score per grade)")]
    public int scoreAPlus = 3500;
    public int scoreA     = 2800;
    public int scoreBPlus = 2200;
    public int scoreB     = 1700;
    public int scoreCPlus = 1300;
}
