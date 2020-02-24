using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JakkaruBurst : WeaponGenerationInfo
{
    public JakkaruBurst()
    {
        name = "Jakkaru Burst";
        description = "A light carbine burst fire weapon.";

        MinSpread = 0f;
        MaxSpread = 3f;

        MinDamage = 10;
        MaxDamage = 20;

        StunTime = 0.3f;
        
        MinKnockbackImpulse = 0;
        MaxKnockbackImpulse = 0;

        MinKnockbackTime = 0.0f;
        MaxKnockbackTime = 0.0f;

        MinClipSize = 15;
        MaxClipSize = 30;

        MinRateOfFire = 0.07f;
        MaxRateOfFire = 0.2f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 10f;
        MaxRange = 18f;

        MaxBulletVeloc = 8f;
        MinBulletVeloc = 14f;
    }
}
