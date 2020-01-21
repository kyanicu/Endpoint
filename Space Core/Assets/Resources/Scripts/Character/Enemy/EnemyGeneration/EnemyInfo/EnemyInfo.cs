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
    public float PatrolRange;
}
