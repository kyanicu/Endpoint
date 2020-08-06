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
<<<<<<< HEAD
        SpeedHi = 5f;
        SpeedLow = 3f;
=======
        SpeedHi = 3f;
        SpeedLow = 1.5f;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        PatrolRange = 3.0f;
    }
}
