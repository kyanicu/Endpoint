/// <summary>
/// Class that holds all information for generating enemies randomly
/// @Author: Adam Federbusch
/// </summary>
public static class EnemyInfo
{
    public static string[] EnemyTypes = { "medium", "small", "large"};

    #region MediumEnemyStats
    public static int MediumEnemyHealthHi = 115;
    public static int MediumEnemyHealthLo = 85;
    public static float MediumEnemySpeedHi = 4f;
    public static float MediumEnemySpeedLo = 2f;
    #endregion

    #region SmallEnemyStats
    public static int SmallEnemyHealthHi = 105;
    public static int SmallEnemyHealthLo = 75;
    public static float SmallEnemySpeedHi = 6f;
    public static float SmallEnemySpeedLo = 3f;
    #endregion

    #region LargeEnemyStats
    public static int LargeEnemyHealthHi = 150;
    public static int LargeEnemyHealthLo = 105;
    public static float LargeEnemySpeedHi = 3f;
    public static float LargeEnemySpeedLo = 1.5f;
    #endregion
}
