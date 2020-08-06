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
<<<<<<< HEAD
        SpeedHi = 9f;
        SpeedLow = 7f;
=======
        SpeedHi = 4f;
        SpeedLow = 2f;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        PatrolRange = 4.0f;
    }
}
