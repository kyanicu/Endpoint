using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class that holds the ranges for all weapon type stats
/// </summary>
public abstract class WeaponGenerationInfo
{
    #region Constant Range Checkers For RNG Name Generation
    protected const float MAX_AVG_RANGE_CHECK = .8f;
    protected const float MIN_AVG_RANGE_CHECK = .25f;
    protected const float MAX_ABOVE_AVG_RANGE_CHECK = .6f;
    protected const float MIN_BELOW_AVG_RANGE_CHECK = .4f;
    protected const float MAX_RANGE_CHECK = .85f;
    protected const float MIN_RANGE_CHECK = .15f;
    #endregion

    public string name;
    public string baseName;
    public string description;

    #region Weapon Stat Ranges
    public float MinSpread;
    public float MaxSpread;

    public int MinDamage;
    public int MaxDamage;
  
    public float StunTime;

    public float MinKnockbackImpulse;
    public float MaxKnockbackImpulse;

    public float MinKnockbackTime;
    public float MaxKnockbackTime;

    public int MinClipSize;
    public int MaxClipSize;

    public float MinRateOfFire;
    public float MaxRateOfFire;

    public float MinReloadTime;
    public float MaxReloadTime;

    public float MinRange;
    public float MaxRange;

    public float MaxBulletVeloc;
    public float MinBulletVeloc;

    public int MinNumPellets = 0;
    public int MaxNumPellets = 0;
    #endregion 

    /// <summary>
    /// Pass its max stats to be compared in GameManager.cs
    /// </summary>
    /// <returns></returns>
    public float[] PassMaxValues()
    {
        float[] maxValues = { MaxDamage, MaxRateOfFire, MaxReloadTime, MaxRange, MaxBulletVeloc };
        return maxValues;
    }

    /// <summary>
    /// Loop through created weapon's stats and return a name appendment
    /// </summary>
    /// <param name="wep"></param>
    /// <returns></returns>
    public string GenerateNewWeaponName(Weapon wep)
    {
        //Check if all stats are in really good range
        if ((wep.Damage - (float)MinDamage) / (MaxDamage - MinDamage) > MAX_AVG_RANGE_CHECK &&
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
