using UnityEngine;

/// <summary>
/// Class that when attached to a gameobject will genearate a random enemy based on the GenerateEnemy method on startup
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemy();
    }

    /// <summary>
    /// Selects an enemy to randomly generates it then creates the enemy
    /// </summary>
    private void GenerateEnemy()
    {
        int enemyTypeIndex = Random.Range(0, EnemyInfo.EnemyTypes.Length);
        string enemyType = EnemyInfo.EnemyTypes[enemyTypeIndex];

        switch (enemyType)
        {
            case "small":
                GenerateSmallEnemy();
                break;
            case "medium":
                GenerateMediumEnemy();
                break;
            case "large":
                GenerateLargeEnemy();
                break;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Generates a random large enemy based on stats stored in EnemyInfo
    /// </summary>
    private void GenerateLargeEnemy()
    {
        GameObject enemy = Resources.Load<GameObject>("Prefabs/Enemy/LargeEnemy");
        GameObject instantiatedEnemy = GameObject.Instantiate(enemy, transform.position, Quaternion.identity);
        LargeEnemy largeEnemy = instantiatedEnemy.GetComponent<LargeEnemy>();
        largeEnemy.MaxHealth = Random.Range(EnemyInfo.LargeEnemyHealthLo, EnemyInfo.LargeEnemyHealthHi);
        largeEnemy.Speed = Random.Range(EnemyInfo.LargeEnemySpeedLo, EnemyInfo.LargeEnemySpeedHi);
        largeEnemy.Health = largeEnemy.MaxHealth;
        largeEnemy.Class = "large";
    }

    /// <summary>
    /// Generates a random medium enemy based on stats stored in EnemyInfo
    /// </summary>
    private void GenerateMediumEnemy()
    {
        GameObject enemy = Resources.Load<GameObject>("Prefabs/Enemy/MediumEnemy");
        GameObject instantiatedEnemy = GameObject.Instantiate(enemy, transform.position, Quaternion.identity);
        MediumEnemy mediumEnemy = instantiatedEnemy.GetComponent<MediumEnemy>();
        mediumEnemy.MaxHealth = Random.Range(EnemyInfo.MediumEnemyHealthLo, EnemyInfo.MediumEnemyHealthHi);
        mediumEnemy.Speed = Random.Range(EnemyInfo.MediumEnemySpeedLo, EnemyInfo.MediumEnemySpeedHi);
        mediumEnemy.Health = mediumEnemy.MaxHealth;
        mediumEnemy.Class = "medium";
    }

    /// <summary>
    /// Generates a random medium enemy based on stats stored in EnemyInfo
    /// </summary>
    private void GenerateSmallEnemy()
    {
        GameObject enemy = Resources.Load<GameObject>("Prefabs/Enemy/SmallEnemy");
        GameObject instantiatedEnemy = GameObject.Instantiate(enemy, transform.position, Quaternion.identity);
        SmallEnemy smallEnemy = instantiatedEnemy.GetComponent<SmallEnemy>();
        smallEnemy.MaxHealth = Random.Range(EnemyInfo.SmallEnemyHealthLo, EnemyInfo.SmallEnemyHealthHi);
        smallEnemy.Speed = Random.Range(EnemyInfo.SmallEnemySpeedLo, EnemyInfo.SmallEnemySpeedHi);
        smallEnemy.Health = smallEnemy.MaxHealth;
        smallEnemy.Class = "small";
    }
}
