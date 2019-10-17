using System;
using UnityEngine;

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
            default:
                return null;
        }
    }

    private static GameObject BuildPrecisionWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Precision");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = 0.0f;
        precision.Damage = UnityEngine.Random.Range(10, 50);
        precision.ClipSize = UnityEngine.Random.Range(1, 10);
        precision.AmmoInClip = precision.ClipSize;
        precision.MaxAmmoCapacity = precision.ClipSize * 4;
        precision.RateOfFire = UnityEngine.Random.Range(0.2f, 1f);
        precision.ReloadTime = UnityEngine.Random.Range(0.5f, 3.0f);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        return weaponObject;
    }

    private static GameObject BuildAutomaticWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Automatic");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(0.0f, 0.02f);
        automatic.Damage = UnityEngine.Random.Range(5, 15);
        automatic.ClipSize = UnityEngine.Random.Range(12, 40);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = automatic.ClipSize * 4;
        automatic.RateOfFire = UnityEngine.Random.Range(0.07f, 0.2f);
        automatic.ReloadTime = UnityEngine.Random.Range(0.5f, 3.0f);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        return weaponObject;
    }

    private static GameObject BuildSpreadWeapon(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Spread");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(0.07f, 0.14f);
        spread.Damage = UnityEngine.Random.Range(3, 10);
        spread.ClipSize = UnityEngine.Random.Range(6, 14);
        spread.AmmoInClip = spread.ClipSize;
        spread.MaxAmmoCapacity = spread.ClipSize * 4;
        spread.RateOfFire = UnityEngine.Random.Range(0.25f, 1.0f);
        spread.ReloadTime = UnityEngine.Random.Range(1.0f, 4.0f);
        spread.NumPellets = UnityEngine.Random.Range(4, 8);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        return weaponObject;
    }
}
