using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyTests
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestEnemyInitialization()
    {
        // Light Enemy
        GameObject lightEnemyGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/LightEnemy"));
        Assert.True(lightEnemyGameObject);
        Object.Destroy(lightEnemyGameObject);

        // Medium Enemy
        GameObject mediumEnemyGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/MediumEnemy"));
        Assert.True(mediumEnemyGameObject);
        Object.Destroy(mediumEnemyGameObject);

        // Heavy Enemy
        GameObject heavyEnemyGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/HeavyEnemy"));
        Assert.True(heavyEnemyGameObject);
        Object.Destroy(heavyEnemyGameObject);
        yield return null;
    }
}
