using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemyInfo : EnemyGenerationInfo
{
    public LightEnemyInfo()
    {
        Class = "light";
        PrefabPath = "Prefabs/Enemy/LightEnemy";
        HealthHi = 105;
        HealthLow = 75;
        SpeedHi = 6f;
        SpeedLow = 3f;
    }
}
