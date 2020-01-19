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
        int enemyTypeIndex = Random.Range(0, Enemy.EnemyTypes.Length);
        string enemyType = Enemy.EnemyTypes[enemyTypeIndex];
        EnemyGenerationInfo info = null;

        switch (enemyType)
        {
            case "light":
                info = new LightEnemyInfo();
                GenerateEnemy(info);
                break;
            case "medium":
                info = new MediumEnemyInfo();
                GenerateEnemy(info);
                break;
            case "heavy":
                info = new HeavyEnemyInfo();
                GenerateEnemy(info);
                break;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Generates a random large enemy based on stats stored in EnemyInfo
    /// </summary>
    private void GenerateEnemy(EnemyGenerationInfo info)
    {
        GameObject enemy = Resources.Load<GameObject>(info.PrefabPath);
        GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        Enemy enemyComponent = instantiatedEnemy.GetComponent<Enemy>();
        enemyComponent.MaxHealth = Random.Range(info.HealthLow, info.HealthHi);
        enemyComponent.Speed = Random.Range(info.SpeedLow, info.SpeedHi);
        enemyComponent.Health = enemyComponent.MaxHealth;
        enemyComponent.Class = info.Class;
    }
}
