using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Levels/LevelsData")]
public class LevelSO : ScriptableObject
{
    public string name;
    public string sceneToLoad;
    public string isUnlockedByDefault;
}
