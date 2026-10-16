using UnityEngine;

/// <summary>
/// Turns the final score into a letter rank: A+ > A > B+ > B > C+ > C.
/// The thresholds (minimum score for each grade, anything below C+ is a C)
/// come from the level's LevelSO, so every level can have its own scale.
/// </summary>
public static class LevelRank
{
    // Used when no LevelSO is known (e.g. when you press Play directly in a level scene)
    private const int DefaultAPlus = 3500;
    private const int DefaultA = 2800;
    private const int DefaultBPlus = 2200;
    private const int DefaultB = 1700;
    private const int DefaultCPlus = 1300;

    public static string Evaluate(int score, LevelSO level)
    {
        int aPlus = level != null ? level.scoreAPlus : DefaultAPlus;
        int a = level != null ? level.scoreA : DefaultA;
        int bPlus = level != null ? level.scoreBPlus : DefaultBPlus;
        int b = level != null ? level.scoreB : DefaultB;
        int cPlus = level != null ? level.scoreCPlus : DefaultCPlus;

        if (score >= aPlus) return "A+";
        if (score >= a) return "A";
        if (score >= bPlus) return "B+";
        if (score >= b) return "B";
        if (score >= cPlus) return "C+";
        return "C";
    }

    public static Color ColorFor(string rank)
    {
        switch (rank)
        {
            case "A+": return new Color(1f, 0.84f, 0.1f);    // gold
            case "A":  return new Color(1f, 0.6f, 0.1f);     // orange
            case "B+": return new Color(0.3f, 0.9f, 0.4f);   // green
            case "B":  return new Color(0.3f, 0.8f, 0.95f);  // cyan
            case "C+": return new Color(0.8f, 0.8f, 0.8f);   // light grey
            default:   return new Color(0.75f, 0.5f, 0.35f); // bronze
        }
    }
}
