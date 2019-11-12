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

    #region Automatic Weapons
    /// <summary>
    /// Stat ranges for Jakkaru (Automatic)
    /// </summary>
    public static class Jakkaru
    {
        public static string name = "Jakkaru";

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

        public static float MinRange = 15f;
        public static float MaxRange = 23f;

        public static float MaxBulletVeloc = 8f;
        public static float MinBulletVeloc = 14f;

        /// <summary>
        /// Generate a random Jakkaru given its stat ranges
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        public static string GenerateAutomaticName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire,
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }

        /// <summary>
        /// Pass its max stats to be compared later
        /// </summary>
        /// <returns></returns>
        public static float[] PassMaxValues()
        {
            float[] maxValues = { MaxDamage, MaxRateOfFire, MaxReloadTime, MaxClipSize, MaxRange, MaxBulletVeloc };
            return maxValues;
        }
    }

    #endregion

    #region Scatter Weapons
    /// <summary>
    /// Stat ranges for a Matsya (Spread)
    /// </summary>
    public static class Matsya
    {
        public static string name = "Matsya";

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

        public static float MinRange = 17f;
        public static float MaxRange = 22f;

        public static float MaxBulletVeloc = 6f;
        public static float MinBulletVeloc = 12f;

        /// <summary>
        /// Generate a random Matsya given its stat ranges
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        public static string GenerateSpreadName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire, 
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }

        /// <summary>
        /// Pass its max stats to be compared later
        /// </summary>
        /// <returns></returns>
        public static float[] PassMaxValues()
        {
            float[] maxValues = { MaxDamage, MaxRateOfFire, MaxReloadTime, MaxClipSize, MaxRange, MaxBulletVeloc };
            return maxValues;
        }
    }

    #endregion

    #region Precision Weapons
    /// <summary>
    /// Stat ranges for a snipeyBoi (Precision)
    /// </summary>

    public static class SnipeyBoi
    {
        public static string name = "SnipeyBoi";

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

        public static float MinRange = 45f;
        public static float MaxRange = 100f;

        public static float MaxBulletVeloc = 20f;
        public static float MinBulletVeloc = 30f;


        /// <summary>
        /// Generate a random snipeyBoi given its stat ranges
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        public static string GeneratePrecisionName(Weapon wep)
        {
            if (!generateNames) return "";
            return generateNewWeaponName(wep, MaxDamage, MinDamage, MaxClipSize, MinClipSize, MaxRateOfFire, MinRateOfFire,
                                         MaxReloadTime, MaxReloadTime, MaxRange, MinRange, MaxBulletVeloc, MinBulletVeloc);
        }

        /// <summary>
        /// Pass its max stats to be compared later
        /// </summary>
        /// <returns></returns>
        public static float[] PassMaxValues()
        {
            float[] maxValues = { MaxDamage, MaxRateOfFire, MaxReloadTime, MaxClipSize, MaxRange, MaxBulletVeloc };
            return maxValues;
        }
    }

    #endregion

    #region Total Range Stats
    /// <summary>
    /// Lowest and highest for each of all Weapons
    /// </summary>
    public static class TotalRangeStats
    {
        public enum MaxStat
        {
            Damage,
            FireRate,
            ReloadTime,
            MagazineSize,
            Range,
            BulletVeloc
        };

        public static float[] MaxValues = { 0, 0, 0, 0, 0 };

        /// <summary>
        /// One time call to load max values for all weapon stats, used when calculating diagnostic bar fill
        /// </summary>
        public static void LoadMaxStats()
        {
            //Load max Jakkaru stats
            float[] loadedStats = Jakkaru.PassMaxValues();
            for (int x = 0; x < (int) MaxStat.ReloadTime; x++)
            {
                if (MaxValues[x] < loadedStats[x])
                {
                    MaxValues[x] = loadedStats[x];
                }
            }

            //Compare max Jakkaru stats to max Matsya stats
            loadedStats = Matsya.PassMaxValues();
            for (int x = 0; x < (int)MaxStat.ReloadTime; x++)
            {
                if (MaxValues[x] < loadedStats[x])
                {
                    MaxValues[x] = loadedStats[x];
                }
            }

            //Compare max SnipeyBoi stats to previous max stats
            loadedStats = SnipeyBoi.PassMaxValues();
            for (int x = 0; x < (int)MaxStat.ReloadTime; x++)
            {
                if (MaxValues[x] < loadedStats[x])
                {
                    MaxValues[x] = loadedStats[x];
                }
            }
        }
    }

    #endregion

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
