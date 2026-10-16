public static class ScoreManager
{
    public static int TotalScore = 0;

    // Fired whenever the score changes, so the HUD can refresh (kills AND boss hits)
    public static event System.Action OnScoreChanged;

    public static void AddScore(int amount)
    {
        TotalScore += amount;
        if (OnScoreChanged != null) OnScoreChanged();
    }

    public static void ResetScore()
    {
        TotalScore = 0;
        if (OnScoreChanged != null) OnScoreChanged();
    }
}
