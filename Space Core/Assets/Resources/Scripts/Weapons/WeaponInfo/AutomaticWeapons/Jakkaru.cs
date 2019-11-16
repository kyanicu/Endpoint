﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jakkaru : WeaponGenerationInfo
{
    public Jakkaru()
    {
        name = "Jakkaru";

        MinSpread = 0f;
        MaxSpread = 3f;

        MinDamage = 5;
        MaxDamage = 15;

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