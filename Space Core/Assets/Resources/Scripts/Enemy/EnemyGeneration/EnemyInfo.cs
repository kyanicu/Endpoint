/// <summary>
/// Class that holds all information for generating enemies randomly
/// </summary>
public static class EnemyInfo
{
    public static string[] EnemyTypes = { "medium", "small", "large"};

    #region MediumEnemyStats
    public static int MediumEnemyHealthHi = 115;
    public static int MediumEnemyHealthLo = 85;
    public static float MediumEnemySpeedHi = 2f;
    public static float MediumEnemySpeedLo = 4f;
    #endregion

    #region SmallEnemyStats
    public static int SmallEnemyHealthHi = 105;
    public static int SmallEnemyHealthLo = 75;
    public static float SmallEnemySpeedHi = 3f;
    public static float SmallEnemySpeedLo = 6f;
    #endregion

    #region
    public static int LargeEnemyHealthHi = 150;
    public static int LargeEnemyHealthLo = 105;
    public static float LargeEnemySpeedHi = 4f;
    public static float LargeEnemySpeedLo = 6f;
    #endregion
}
