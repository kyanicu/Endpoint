using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeyBoi : WeaponGenerationInfo
{
    public SnipeyBoi()
    {
        name = "SnipeyBoi";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 10;
        MaxDamage = 50;

        MinClipSize = 1;
        MaxClipSize = 10;

        MinRateOfFire = 0.25f;
        MaxRateOfFire = 1.0f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 45f;
        MaxRange = 100f;

        MaxBulletVeloc = 20f;
        MinBulletVeloc = 30f;
    }
}
