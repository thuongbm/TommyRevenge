using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Levels/LevelsData")]
public class LevelSO : ScriptableObject
{
    public string name;
    public string sceneToLoad;
    public int enemyTotal;
    public float parTime = 60f;

    public string isUnlockedByDefault;
}
