using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterSO", menuName = "Chapters/ChaptersData")]
public class ChapterSO : ScriptableObject
{
    public string title;
    public List<LevelSO> levels = new List<LevelSO>(); 
}
