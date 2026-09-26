public static class ScoreManager
{
    public static int TotalScore = 0;

    public static void AddScore(int amount)
    {
        TotalScore += amount;
    }

    public static void ResetScore()
    {
        TotalScore = 0;
    }
}
