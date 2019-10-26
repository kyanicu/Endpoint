﻿/// <summary>
/// Class that holds all information for generating enemies randomly
/// </summary>
public static class EnemyInfo
{
    public static string[] EnemyTypes = { "medium", "small" };

    #region MediumEnemyStats
    public static int MediumEnemyHealthHi = 115;
    public static int MediumEnemyHealthLo = 85;
    public static float MediumEnemySpeedHi = 6f;
    public static float MediumEnemySpeedLo = 8f;
    #endregion

    #region SmallEnemyStats
    public static int SmallEnemyHealthHi = 105;
    public static int SmallEnemyHealthLo = 75;
    public static float SmallEnemySpeedHi = 8f;
    public static float SmallEnemySpeedLo = 10f;
    #endregion
}
