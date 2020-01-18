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
        SpeedHi = 3f;
        SpeedLow = 1.5f;
    }
}
