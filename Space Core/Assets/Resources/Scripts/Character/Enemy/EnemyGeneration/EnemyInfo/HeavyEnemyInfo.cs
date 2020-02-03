using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemyInfo : EnemyGenerationInfo
{
    public HeavyEnemyInfo()
    {
        Class = "heavy";
        PrefabPath = "Prefabs/Enemy/HeavyEnemy";
        HealthHi = 150;
        HealthLow = 105;
        SpeedHi = 5f;
        SpeedLow = 3f;
        PatrolRange = 3.0f;
    }
}
