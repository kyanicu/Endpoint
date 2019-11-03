using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class that holds the ranges for all weapon type stats
/// </summary>
public static class WeaponGenerationInfo
{
    #region Constant Range Checkers For RNG Name Generation
    const float MAX_AVG_RANGE_CHECK = .8f;
    const float MIN_AVG_RANGE_CHECK = .25f;
    const float MAX_ABOVE_AVG_RANGE_CHECK = .6f;
    const float MIN_BELOW_AVG_RANGE_CHECK = .4f;
    const float MAX_RANGE_CHECK = .85f;
    const float MIN_RANGE_CHECK = .15f;
    #endregion

    //Set to false if you want names to be static
    private static bool generateNames = true;

    /// <summary>
    /// Stat ranges for Automatic Weapons
    /// </summary>
    public static class AutomaticStats
    {
        public static Weapon.WeaponType type = Weapon.WeaponType.Automatic;
        public static string name = "Automatic";

        public static float MinSpread = 0f;
        public static float MaxSpread = 3f;

        public static int MinDamage = 5;
        public static int MaxDamage = 15;

        public static int MinClipSize = 12;
        public static int MaxClipSize = 40;

        public static float MinRateOfFire = 0.07f;
        public static float MaxRateOfFire = 0.2f;

        public static float MinReloadTime = 0.5f;
        public static float MaxReloadTime = 3.0f;

        public static float MaxRange = 15f;
        public static float MinRange = 23f;

        public static float MaxBulletVeloc = 14f;
        public static float MinBulletVeloc = 28f;

        public static string GenerateAutomaticName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire,
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }
    }

    /// <summary>
    /// Stat ranges for Spread Weapons
    /// </summary>
    public static class SpreadStats
    {
        public static Weapon.WeaponType type = Weapon.WeaponType.Spread;
        public static string name = "Spread";

        public static float MinSpread = 7f;
        public static float MaxSpread = 14f;

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

        public static float MaxRange = 17f;
        public static float MinRange = 22f;

        public static float MaxBulletVeloc = 12f;
        public static float MinBulletVeloc = 22f;

        public static string GenerateSpreadName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire, 
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }
    }

    /// <summary>
    /// Stat ranges for Precision Weapons
    /// </summary>

    public static class PrecisionStats
    {
        public static Weapon.WeaponType type = Weapon.WeaponType.Precision;
        public static string name = "Precision";

        public static float MinSpread = 0.0f;
        public static float MaxSpread = 0.0f;

        public static int MinDamage = 10;
        public static int MaxDamage = 50;

        public static int MinClipSize = 1;
        public static int MaxClipSize = 10;

        public static float MinRateOfFire = 0.25f;
        public static float MaxRateOfFire = 1.0f;

        public static float MinReloadTime = 0.5f;
        public static float MaxReloadTime = 3.0f;

        public static float MaxRange = 45f;
        public static float MinRange = 100f;

        public static float MaxBulletVeloc = 20f;
        public static float MinBulletVeloc = 40f;

        public static string GeneratePrecisionName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire,
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }
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

    /// <summary>
    /// Loop through created weapon's stats and return a name appendment
    /// </summary>
    /// <param name="wep"></param>
    /// <returns></returns>
    private static string generateNewWeaponName(Weapon wep, int MaxDamage, int MinDamage, int MaxClipSize, int MinClipSize, float MaxRateOfFire, float MinRateOfFire,
                                                 float MaxReloadTime, float MinReloadTime, float MaxRange, float MinRange, float MaxBulletVeloc, float MinBulletVeloc)
    {
        //Check if all stats are in really good range
        if ((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) > MAX_AVG_RANGE_CHECK &&
                 (wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) > MAX_AVG_RANGE_CHECK &&
                 (wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) > MAX_AVG_RANGE_CHECK &&
                 (wep.Range - MinRange) / (MaxRange - MinRange) > MAX_AVG_RANGE_CHECK &&
                 (wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc) > MAX_AVG_RANGE_CHECK &&
                 (wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) > MAX_AVG_RANGE_CHECK)
        {
            string[] BestNames = { "Legendary ", "Godly ", "Unique ", "Optimal ", "Ideal" };
            int rngVal = Random.Range(0, BestNames.Length);
            return BestNames[rngVal];
        }
        //Check if all stats are in really terrible range
        else if ((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) < MIN_AVG_RANGE_CHECK &&
                 (wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) < MIN_AVG_RANGE_CHECK &&
                 (wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) < MIN_AVG_RANGE_CHECK &&
                 (wep.Range - MinRange) / (MaxRange - MinRange) < MIN_AVG_RANGE_CHECK &&
                 (wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc) < MIN_AVG_RANGE_CHECK &&
                 (wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) < MIN_AVG_RANGE_CHECK)
        {
            string[] WorstNames = { "Tattered ", "Rusted ", "Scrapped ", "Broken ", "Useless" };
            int rngVal = Random.Range(0, WorstNames.Length);
            return WorstNames[rngVal];
        }
        //Check if just damage is good
        else if ((wep.Damage - (float) MinDamage) / (MaxDamage - MinDamage) > MAX_RANGE_CHECK)
        {
            return "Powerful ";
        }
        //Check if just damage is bad
        else if ((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) < MIN_RANGE_CHECK)
        {
            return "Pathetic ";
        }
        //Check if just ClipSize is good
        else if ((wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) > MAX_RANGE_CHECK)
        {
            return "Loaded ";
        }
        //Check if just ClipSize is bad
        else if ((wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) < MIN_RANGE_CHECK)
        {
            return "Hollow ";
        }
        //Check if just RateOfFire is good
        else if ((wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) > MAX_RANGE_CHECK)
        {
            return "Celeritous ";
        }
        //Check if just RateOfFire is bad
        else if ((wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) < MIN_RANGE_CHECK)
        {
            return "Sluggish ";
        }
        //Check if just ReloadTime is good
        else if ((wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) > MAX_RANGE_CHECK)
        {
            return "Ergonomic ";
        }
        //Check if just ReloadTime is bad
        else if ((wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) < MIN_RANGE_CHECK)
        {
            return "Clumsy ";
        }
        //Check if just Range is good
        else if ((wep.Range - MinRange) / (MaxRange - MinRange) > MAX_RANGE_CHECK)
        {
            return "Scoped ";
        }
        //Check if just Range is bad
        else if ((wep.Range - MinRange) / (MaxRange - MinRange) < MIN_RANGE_CHECK)
        {
            return "Blind ";
        }
        //Check if just BulletVeloc is good
        else if ((wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc) > MAX_RANGE_CHECK)
        {
            return "Dispensing ";
        }
        //Check if just BulletVeloc is bad
        else if ((wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc) < MIN_RANGE_CHECK)
        {
            return "Slow ";
        }
        //Check if stats are in above average range
        else if (((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) +
                  (wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) +
                  (wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) +
                  (wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) +
                  (wep.Range - MinRange) / (MaxRange - MinRange) +
                  (wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc)) / 6 > MAX_ABOVE_AVG_RANGE_CHECK)
        {
            string[] AboveAvgNames = { "Modified ", "Upgraded ", "Advanced ", "Improved ", "Enhanced" };
            int rngVal = Random.Range(0, AboveAvgNames.Length);
            return AboveAvgNames[rngVal];
        }
        //Check if stats are in below average range
        else if (((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) +
                  (wep.ClipSize - (float)MinClipSize) / (MaxClipSize - MinClipSize) +
                  (wep.RateOfFire - MinRateOfFire) / (MaxRateOfFire - MinRateOfFire) +
                  (wep.ReloadTime - MinReloadTime) / (MaxReloadTime - MinReloadTime) +
                  (wep.Range - MinRange) / (MaxRange - MinRange) +
                  (wep.BulletVeloc - MinBulletVeloc) / (MaxBulletVeloc - MinBulletVeloc)) / 6 < MIN_BELOW_AVG_RANGE_CHECK)
        {
            string[] BelowAvgNames = { "Tested ", "Worn ", "Weathered ", "Battered ", "Damaged" };
            int rngVal = Random.Range(0, BelowAvgNames.Length);
            return BelowAvgNames[rngVal];
        }
        //Otherwise that weapon is a basic bitch
        else
        {
            string[] AverageNames = { "Standard ", "Factory ", "Default ", "Common ", "Basic" };
            int rngVal = Random.Range(0, AverageNames.Length);
            return AverageNames[rngVal];
        }
    }
}
