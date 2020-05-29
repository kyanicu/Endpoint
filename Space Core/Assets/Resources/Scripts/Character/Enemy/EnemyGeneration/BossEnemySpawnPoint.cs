using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Selects an enemy to randomly generates it then creates the enemy
    /// </summary>
    public void GenerateEnemy()
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
    }

    /// <summary>
    /// Generates a random large enemy based on stats stored in EnemyInfo
    /// </summary>
    private void GenerateEnemy(EnemyGenerationInfo info)
    {
        GameObject enemy = Resources.Load<GameObject>(info.PrefabPath);
        GameObject loadedEnemyController = null;
        loadedEnemyController = Resources.Load<GameObject>("Prefabs/Enemy/AIControllers/AIController");
        GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        GameObject instantiatedAIController = Instantiate(loadedEnemyController);
        AIController aiController = null;
        switch (info.Class)
        {
            case "heavy":
                instantiatedAIController.GetComponent<HeavyEnemyAIController>().enabled = true;
                aiController = instantiatedAIController.GetComponent<HeavyEnemyAIController>();
                break;
            case "medium":
                instantiatedAIController.GetComponent<MediumEnemyAIController>().enabled = true;
                aiController = instantiatedAIController.GetComponent<MediumEnemyAIController>();
                break;
            case "light":
                instantiatedAIController.GetComponent<LightEnemyAIController>().enabled = true;
                aiController = instantiatedAIController.GetComponent<LightEnemyAIController>();
                break;
        }
        aiController.Character = instantiatedEnemy.GetComponent<Character>();
        aiController.Character.MaxHealth = Random.Range(info.HealthLow, info.HealthHi);
        aiController.Character.Health = aiController.Character.MaxHealth;
        aiController.Character.Class = info.Class;
        aiController.Character.movement.runMax = Random.Range(info.SpeedLow, info.SpeedHi);
    }
}
