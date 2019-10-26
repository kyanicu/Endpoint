using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds the ranges for all weapon type stats
/// </summary>
public static class WeaponGenerationInfo
{
    /// <summary>
    /// Stat ranges for Automatic Weapons
    /// </summary>
    public static class AutomaticStats
    {
        public static float MinSpread = 0.0f;
        public static float MaxSpread = 0.02f;

        public static int MinDamage = 5;
        public static int MaxDamage = 15;

        public static int MinClipSize = 12;
        public static int MaxClipSize = 40;

        public static float MinRateOfFire = 0.07f;
        public static float MaxRateOfFire = 0.2f;

        public static float MinReloadTime = 0.5f;
        public static float MaxReloadTime = 3.0f;
    }

    /// <summary>
    /// Stat ranges for Spread Weapons
    /// </summary>
    public static class SpreadStats
    {
        public static float MinSpread = 0.07f;
        public static float MaxSpread = 0.14f;

        public static int MinDamage = 3;
        public static int MaxDamage = 10;

        public static int MinClipSize = 6;
        public static int MaxClipSize = 14;

        public static float MinRateOfFire = 0.25f;
        public static float MaxRateOfFire = 1.0f;

        public static float MinReloadTime = 1.0f;
        public static float MaxReloadTime = 4.0f;

        public static int MinNumPellets = 4;
        public static int MaxNumPellets = 8;
    }

    /// <summary>
    /// Stat ranges for Precision Weapons
    /// </summary>
    public static class PrecisionStats
    {
        public static float MinSpread = 0.0f;
        public static float MaxSpread = 0.0f;

        public static int MinDamage = 10;
        public static int MaxDamage = 50;

        public static int MinClipSize = 1;
        public static int MaxClipSize = 10;

        public static float MinRateOfFire = 0.07f;
        public static float MaxRateOfFire = 0.2f;

        public static float MinReloadTime = 0.5f;
        public static float MaxReloadTime = 3.0f;
    }

    /// <summary>
    /// Lowest and highest for each of all Weapons
    /// </summary>
    public static class TotalRangeStats
    {
        public static float MinSpread = 0.0f;
        public static float MaxSpread = 0.14f;

        public static int MinDamage = 5;
        public static int MaxDamage = 50;

        public static int MinClipSize = 1;
        public static int MaxClipSize = 15;

        public static float MinRateOfFire = 0.07f;
        public static float MaxRateOfFire = 1f;

        public static float MinReloadTime = 0.5f;
        public static float MaxReloadTime = 4.0f;
    }
}
