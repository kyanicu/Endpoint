/// <summary>
/// Class that holds all information for generating enemies randomly
/// </summary>
public abstract class EnemyGenerationInfo
{
    public string Class;
    public string PrefabPath;
    public int HealthHi;
    public int HealthLow;
    public float SpeedHi;
    public float SpeedLow;

    /*
    #region MediumEnemyStats
    public static int MediumEnemyHealthHi = 115;
    public static int MediumEnemyHealthLo = 85;
    public static float MediumEnemySpeedHi = 4f;
    public static float MediumEnemySpeedLo = 2f;
    #endregion

    #region SmallEnemyStats
    public static int LightEnemyHealthHi = 105;
    public static int LightEnemyHealthLo = 75;
    public static float LightEnemySpeedHi = 6f;
    public static float LightEnemySpeedLo = 3f;
    #endregion

    #region LargeEnemyStats
    public static int HeavyEnemyHealthHi = 150;
    public static int HeavyEnemyHealthLo = 105;
    public static float HeavyEnemySpeedHi = 3f;
    public static float HeavyEnemySpeedLo = 1.5f;
    #endregion
    */
}
