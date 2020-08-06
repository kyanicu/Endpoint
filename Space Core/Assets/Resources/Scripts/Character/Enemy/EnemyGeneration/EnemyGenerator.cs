<<<<<<< HEAD
﻿using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

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
        int enemyTypeIndex = Random.Range(0, EnemyController.EnemyTypes.Length);
        string enemyType = EnemyController.EnemyTypes[enemyTypeIndex];
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
        GameObject loadedEnemyController = Resources.Load<GameObject>("Prefabs/Enemy/EnemyController");
        GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        GameObject instantiatedEnemyController = Instantiate(loadedEnemyController);
        EnemyController enemyController = instantiatedEnemyController.GetComponent<EnemyController>();
        enemyController.Character = instantiatedEnemy.GetComponent<Character>();
        enemyController.Character.MaxHealth = Random.Range(info.HealthLow, info.HealthHi);
        enemyController.Speed = Random.Range(info.SpeedLow, info.SpeedHi);
        enemyController.PatrolRange = info.PatrolRange;
        enemyController.Character.Health = enemyController.Character.MaxHealth;
        enemyController.Character.Class = info.Class;
    }
}
