using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that when attached to a gameobject will genearate a random enemy based on the GenerateEnemy method on startup
/// </summary>
public class AIGenerator : MonoBehaviour
{
    public List<Node> RoomGraph;

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
        int enemyTypeIndex = Random.Range(0, EnemyController.EnemyTypes.Length);
        string enemyType = AIController.EnemyTypes[enemyTypeIndex];
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
        GameObject loadedEnemyController = Resources.Load<GameObject>("Prefabs/Enemy/AIController");
        GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        GameObject instantiatedAIController = Instantiate(loadedEnemyController);
        AIController aiController = instantiatedAIController.GetComponent<AIController>();
        aiController.Character = instantiatedEnemy.GetComponent<Character>();
        aiController.Character.MaxHealth = Random.Range(info.HealthLow, info.HealthHi);
        aiController.Character.Health = aiController.Character.MaxHealth;
        aiController.Character.Class = info.Class;
        aiController.RoomGraph = RoomGraph;
        aiController.Character.movement.runMax = Random.Range(info.SpeedLow, info.SpeedHi);
    }
}
