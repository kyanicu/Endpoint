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
<<<<<<< HEAD
        SpeedHi = 11f;
        SpeedLow = 8f;
=======
        SpeedHi = 6f;
        SpeedLow = 3f;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        PatrolRange = 8.0f;
    }
}
