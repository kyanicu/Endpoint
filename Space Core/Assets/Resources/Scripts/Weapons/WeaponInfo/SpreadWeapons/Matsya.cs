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

         MinSpread = 7f;
         MaxSpread = 14f;

         MinDamage = 3;
         MaxDamage = 10;

         MinClipSize = 6;
         MaxClipSize = 14;

         MinRateOfFire = 0.25f;
         MaxRateOfFire = 1.0f;

         MinReloadTime = 1.0f;
         MaxReloadTime = 4.0f;

         MinNumPellets = 4;
         MaxNumPellets = 8;

         MinRange = 17f;
         MaxRange = 22f;

         MaxBulletVeloc = 6f;
         MinBulletVeloc = 12f;

    }
}
