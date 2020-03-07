using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseProjectorInfo : WeaponGenerationInfo
{
    /// <summary>
    /// Stat ranges for a Pulse Projector (Spread)
    /// </summary>
    public PulseProjectorInfo()
    {
        name = "Pulse Projector";
        description = "Pump shotgun with a powerful punch.";

        BulletTag = "WaveProjectile";

        MinSpread = 7f;
        MaxSpread = 14f;

        MinDamage = 20;
        MaxDamage = 30;

        StunTime = 0.35f;

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

        MinRange = 6f;
        MaxRange = 10f;

        MaxBulletVeloc = 18f;
        MinBulletVeloc = 9f;

    }
}
