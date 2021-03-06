﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaryRepeaterInfo : WeaponGenerationInfo
{
    public RotaryRepeaterInfo()
    {
        name = "Rotary Repeater";
        description = "A light carbine with high rate of fire.";
        BulletTag = "NormalBullet";

        MinSpread = 0f;
        MaxSpread = 3f;

        MinDamage = 10;
        MaxDamage = 20;

        StunTime = 0.35f;
        
        MinKnockbackImpulse = 10;
        MaxKnockbackImpulse = 10;

        MinKnockbackTime = 0.1f;
        MaxKnockbackTime = 0.1f;

        MinClipSize = 15;
        MaxClipSize = 25;

        MinRateOfFire = 0.07f;
        MaxRateOfFire = 0.2f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 10f;
        MaxRange = 18f;

        MaxBulletVeloc = 14f;
        MinBulletVeloc = 8f;
    }
}
