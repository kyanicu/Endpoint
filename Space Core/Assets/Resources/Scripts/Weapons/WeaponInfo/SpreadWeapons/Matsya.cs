using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsya : WeaponGenerationInfo
{
    /// <summary>
    /// Stat ranges for a Matsya (Spread)
    /// </summary>
    public Matsya()
    {
        name = "Matsya";
        description = "Pump shotgun with a powerful punch.";

        MinSpread = 7f;
        MaxSpread = 14f;

        MinDamage = 20;
        MaxDamage = 30;

        StunTime = 0.1f;

        MinKnockbackImpulse = 10;
        MaxKnockbackImpulse = 10;

        MinKnockbackTime = 0.1f;
        MaxKnockbackTime = 0.1f;

        MinClipSize = 6;
        MaxClipSize = 14;

        MinRateOfFire = 0.25f;
        MaxRateOfFire = 1.0f;

        MinReloadTime = 1.0f;
        MaxReloadTime = 4.0f;

        MinNumPellets = 2;
        MaxNumPellets = 4;

        MinRange = 7f;
        MaxRange = 13f;

        MaxBulletVeloc = 6f;
        MinBulletVeloc = 12f;

    }
}
