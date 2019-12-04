using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeyBoi : WeaponGenerationInfo
{
    public SnipeyBoi()
    {
        name = "SnipeyBoi";
        description = "Typical ranged weapon with no special features.";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 10;
        MaxDamage = 50;

        MinKnockbackImpulse = 5;
        MaxKnockbackImpulse = 20;

        MinKnockbackTime = 0.1f;
        MaxKnockbackTime = 0.75f;

        MinStunTime = 0;
        MinStunTime = 0.3f;

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
