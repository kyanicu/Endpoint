﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jakkaru : WeaponGenerationInfo
{
    public Jakkaru()
    {
        name = "Jakkaru";
        description = "A light carbine with high rate of fire.";

        MinSpread = 0f;
        MaxSpread = 3f;

        MinDamage = 10;
        MaxDamage = 20;

        StunTime = 0.3f;

        MinClipSize = 12;
        MaxClipSize = 40;

        MinRateOfFire = 0.07f;
        MaxRateOfFire = 0.2f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 15f;
        MaxRange = 23f;

        MaxBulletVeloc = 8f;
        MinBulletVeloc = 14f;
    }
}
