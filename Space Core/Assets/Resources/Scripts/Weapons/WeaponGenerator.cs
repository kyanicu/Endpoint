using System;
using UnityEngine;
using automaticStats = WeaponGenerationInfo.AutomaticStats;
using precisionStats = WeaponGenerationInfo.PrecisionStats;
using spreadStats = WeaponGenerationInfo.SpreadStats;

/// <summary>
/// This class will generate a random weapon
/// </summary>
public static class WeaponGenerator
{
    //Used for generating max ammo capacity
    private const int MIN_RNG = 3;
    private const int MAX_RNG = 5;

    /// <summary>
    /// Main function to generate a weapon
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GenerateWeapon(Transform parent)
    {
        //get number of weapons that can be generated
        int num = Enum.GetValues(typeof(Weapon.WeaponType)).Length;
        //parse into a weapon type
        Weapon.WeaponType weaponType = (Weapon.WeaponType) UnityEngine.Random.Range(0, num);

        //Switch on the weapon type
        switch(weaponType)
        {
            case Weapon.WeaponType.Automatic:
                return BuildAutomaticWeapon(parent);
            case Weapon.WeaponType.Spread:
                return BuildSpreadWeapon(parent);
            case Weapon.WeaponType.Precision:
                return BuildPrecisionWeapon(parent);
            default:
                return null;
        }
    }

    /// <summary>
    /// This function will generate a randomly made automatic weapon base on stats from
    /// automatic stats.
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildAutomaticWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Automatic");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(automaticStats.MinSpread, automaticStats.MaxSpread);
        automatic.Damage = UnityEngine.Random.Range(automaticStats.MinDamage, automaticStats.MaxDamage);
        automatic.ClipSize = UnityEngine.Random.Range(automaticStats.MinClipSize, automaticStats.MaxClipSize);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = UnityEngine.Random.Range(automatic.ClipSize * MIN_RNG, automatic.ClipSize * MAX_RNG);
        automatic.RateOfFire = UnityEngine.Random.Range(automaticStats.MinRateOfFire, automaticStats.MaxRateOfFire);
        automatic.ReloadTime = UnityEngine.Random.Range(automaticStats.MinReloadTime, automaticStats.MaxReloadTime);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a randomly made precision weapon base on stats from
    /// automatic stats.
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildPrecisionWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Precision");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = precisionStats.MinSpread;
        precision.Damage = UnityEngine.Random.Range(precisionStats.MinDamage, precisionStats.MaxDamage);
        precision.ClipSize = UnityEngine.Random.Range(precisionStats.MinClipSize, precisionStats.MaxClipSize);
        precision.AmmoInClip = precision.ClipSize;
        precision.MaxAmmoCapacity = UnityEngine.Random.Range(precision.ClipSize * MIN_RNG, precision.ClipSize * MAX_RNG);
        precision.RateOfFire = UnityEngine.Random.Range(precisionStats.MinRateOfFire, precisionStats.MaxRateOfFire);
        precision.ReloadTime = UnityEngine.Random.Range(precisionStats.MinReloadTime, precisionStats.MaxReloadTime);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a randomly made spread weapon base on stats from
    /// automatic stats.
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildSpreadWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Spread");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(spreadStats.MinSpread, spreadStats.MaxSpread);
        spread.Damage = UnityEngine.Random.Range(spreadStats.MinDamage, spreadStats.MaxDamage);
        spread.ClipSize = UnityEngine.Random.Range(spreadStats.MinClipSize, spreadStats.MaxClipSize);
        spread.AmmoInClip = spread.ClipSize;
        spread.MaxAmmoCapacity = UnityEngine.Random.Range(spread.ClipSize * MIN_RNG, spread.ClipSize * MAX_RNG);
        spread.RateOfFire = UnityEngine.Random.Range(spreadStats.MinRateOfFire, spreadStats.MaxRateOfFire);
        spread.ReloadTime = UnityEngine.Random.Range(spreadStats.MinReloadTime, spreadStats.MaxReloadTime);
        spread.NumPellets = UnityEngine.Random.Range(spreadStats.MinNumPellets, spreadStats.MaxNumPellets);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        return weaponObject;
    }
}
