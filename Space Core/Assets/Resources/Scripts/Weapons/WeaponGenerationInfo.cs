using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class that holds the ranges for all weapon type stats
/// </summary>
public static class WeaponGenerationInfo
{
    const float MAX_RANGE_CHECK = .8f;
    const float MIN_RANGE_CHECK = .25f;

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

        /// <summary>
        /// Loop through created weapon's stats and return a name appendment
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        private static string generateNewWeaponName(Weapon wep)
        {
            Automatic a = wep.GetComponent<Automatic>();

            //Check if all stats are in really good range
            if (a.Damage / MaxDamage > MAX_RANGE_CHECK &&
                a.ClipSize / MaxClipSize > MAX_RANGE_CHECK &&
                a.RateOfFire / MaxRateOfFire > MAX_RANGE_CHECK &&
                a.ReloadTime / MaxReloadTime > MAX_RANGE_CHECK)
            {
                return "Legendary" + name;
            }
            //Check if all stats are in really terrible range
            else if (a.Damage / MaxDamage < MIN_RANGE_CHECK &&
                     a.ClipSize / MaxClipSize < MIN_RANGE_CHECK &&
                     a.RateOfFire / MaxRateOfFire < MIN_RANGE_CHECK &&
                     a.ReloadTime / MaxReloadTime < MIN_RANGE_CHECK)
            {
                return "Rusted" + name;
            }

            //After checking total stats, check stats one by one

            return "";
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
