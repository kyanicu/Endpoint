﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussCannonInfo : WeaponGenerationInfo
{
    public GaussCannonInfo()
    {
        name = "Gauss Cannon";
        description = "Typical ranged weapon with no special features.";

        BulletTag = "NormalBullet";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 30;
        MaxDamage = 40;

        StunTime = 0.35f;

        MinKnockbackImpulse = 10;
        MaxKnockbackImpulse = 10;

        MinKnockbackTime = 0.1f;
        MaxKnockbackTime = 0.1f;

        MinClipSize = 5;
        MaxClipSize = 15;

        MinRateOfFire = 0.25f;
        MaxRateOfFire = 1.0f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 15f;
        MaxRange = 25f;

        MaxBulletVeloc = 40f;
        MinBulletVeloc = 30f;
    }
}