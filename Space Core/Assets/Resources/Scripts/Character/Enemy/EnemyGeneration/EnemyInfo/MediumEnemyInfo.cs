using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyInfo : EnemyGenerationInfo
{
    public MediumEnemyInfo()
    {
        Class = "medium";
        PrefabPath = "Prefabs/Enemy/MediumEnemy";
        HealthHi = 115;
        HealthLow = 85;
        SpeedHi = 4f;
        SpeedLow = 2f;
        PatrolRange = 4.0f;
    }
}
