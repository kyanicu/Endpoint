using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachMissileInfo : WeaponGenerationInfo
{
    //Stat ranges for RawketLawnchair Heavy Weapon
    public BreachMissileInfo()
    {
        name = "Breach Missile";
        description = "Standard heavy rocket launcher with no special features.";
        BulletTag = "Rocket";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 30;
        MaxDamage = 45;

        StunTime = 0.1f;

        MinKnockbackImpulse = 17;
        MaxKnockbackImpulse = 17;

        MinClipSize = 1;
        MaxClipSize = 3;

        MinRateOfFire = 1.0f;
        MaxRateOfFire = 1.75f;

        MinReloadTime = 1.0f;
        MaxReloadTime = 5.0f;

        MinRange = 15f;
        MaxRange = 25f;

        MaxBulletVeloc = 19f;
        MinBulletVeloc = 10f;
    }
}
