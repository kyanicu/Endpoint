using System;
using UnityEngine;
using automaticStats = WeaponGenerationInfo.AutomaticStats;
using precisionStats = WeaponGenerationInfo.PrecisionStats;
using spreadStats = WeaponGenerationInfo.SpreadStats;

public static class WeaponGenerator
{
    public static GameObject GenerateWeapon(Transform parent)
    {
        int num = Enum.GetValues(typeof(Weapon.WeaponType)).Length;
        Weapon.WeaponType weaponType = (Weapon.WeaponType) UnityEngine.Random.Range(0, num);

        switch(weaponType)
        {
            case Weapon.WeaponType.Automatic:
                return BuildAutomaticWeapon(parent);
            case Weapon.WeaponType.Spread:
                return BuildSpreadWeapon(parent);
            case Weapon.WeaponType.Precision:
                return BuildPrecisionWeapon(parent);
            case Weapon.WeaponType.Burst:
                return BuildBurstWeapon(parent);
            default:
                return null;
        }
    }

    private static GameObject BuildAutomaticWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Automatic");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(automaticStats.MinSpread, automaticStats.MaxSpread);
        automatic.Damage = UnityEngine.Random.Range(automaticStats.MinDamage, automaticStats.MaxDamage);
        automatic.ClipSize = UnityEngine.Random.Range(automaticStats.MinClipSize, automaticStats.MaxClipSize);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = UnityEngine.Random.Range(automatic.ClipSize * 3, automatic.ClipSize * 5);
        automatic.RateOfFire = UnityEngine.Random.Range(automaticStats.MinRateOfFire, automaticStats.MaxRateOfFire);
        automatic.ReloadTime = UnityEngine.Random.Range(automaticStats.MinReloadTime, automaticStats.MaxReloadTime);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        automatic.ReloadMethod = (Weapon.ReloadType)UnityEngine.Random.Range(0, 2);
        return weaponObject;
    }

    private static GameObject BuildPrecisionWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Precision");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = precisionStats.MinSpread;
        precision.Damage = UnityEngine.Random.Range(precisionStats.MinDamage, precisionStats.MaxDamage);
        precision.ClipSize = UnityEngine.Random.Range(precisionStats.MinClipSize, precisionStats.MaxClipSize);
        precision.AmmoInClip = precision.ClipSize;
        precision.MaxAmmoCapacity = UnityEngine.Random.Range(precision.ClipSize * 3, precision.ClipSize * 5);
        precision.RateOfFire = UnityEngine.Random.Range(precisionStats.MinRateOfFire, precisionStats.MaxRateOfFire);
        precision.ReloadTime = UnityEngine.Random.Range(precisionStats.MinReloadTime, precisionStats.MaxReloadTime);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        precision.ReloadMethod = (Weapon.ReloadType)UnityEngine.Random.Range(0, 2);
        return weaponObject;
    }

    private static GameObject BuildSpreadWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Spread");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(spreadStats.MinSpread, spreadStats.MaxSpread);
        spread.Damage = UnityEngine.Random.Range(spreadStats.MinDamage, spreadStats.MaxDamage);
        spread.ClipSize = UnityEngine.Random.Range(spreadStats.MinClipSize, spreadStats.MaxClipSize);
        spread.AmmoInClip = spread.ClipSize;
        spread.MaxAmmoCapacity = UnityEngine.Random.Range(spread.ClipSize * 3, spread.ClipSize * 5);
        spread.RateOfFire = UnityEngine.Random.Range(spreadStats.MinRateOfFire, spreadStats.MaxRateOfFire);
        spread.ReloadTime = UnityEngine.Random.Range(spreadStats.MinReloadTime, spreadStats.MaxReloadTime);
        spread.NumPellets = UnityEngine.Random.Range(spreadStats.MinNumPellets, spreadStats.MaxNumPellets);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        spread.ReloadMethod = (Weapon.ReloadType) UnityEngine.Random.Range(0, 2);
        return weaponObject;
    }

    private static GameObject BuildBurstWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Burst");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Burst burst = weaponObject.GetComponent<Burst>();
        burst.SpreadFactor = UnityEngine.Random.Range(0.0f, 0.02f);
        burst.Damage = UnityEngine.Random.Range(5, 15);
        burst.ClipSize = UnityEngine.Random.Range(12, 40);
        burst.AmmoInClip = burst.ClipSize;
        burst.MaxAmmoCapacity = burst.ClipSize * 4;
        burst.RateOfFire = UnityEngine.Random.Range(0.07f, 0.2f);
        burst.ReloadTime = UnityEngine.Random.Range(0.5f, 3.0f);
        burst.TotalAmmo = burst.MaxAmmoCapacity;
        burst.BurstAmt = UnityEngine.Random.Range(2, 5);
        burst.SpeedBetweenBurst = UnityEngine.Random.Range(.001f, .2f);
        burst.ReloadMethod = (Weapon.ReloadType)UnityEngine.Random.Range(0, 2);
        return weaponObject;
    }
}
