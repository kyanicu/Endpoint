using System;
using UnityEngine;

/// <summary>
/// This class will generate a random weapon
/// </summary>
public static class WeaponGenerator
{
    //Used for generating max ammo capacity
    public const int MIN_RNG = 3;
    public const int MAX_RNG = 5;

    /// <summary>
    /// Main function to generate a weapon
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GenerateWeapon(Transform parent, string weaponName)
    {
        //Create empty object to load generated weapon into
        GameObject weapon = new GameObject();

       //Create empty string to load generated prefix into
        string prefix = "";

        //Generate a weaon specified by its name
        switch(weaponName)
        {
            case "Jakkaru":
                //Load generated Jakkaru stats onto weapon
                WeaponGenerationInfo newJakkaru = new Jakkaru();
                weapon = BuildAutomatic(parent, newJakkaru);
                prefix = newJakkaru.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Matsya":
                //Load generated Matsya stats onto weapon
                WeaponGenerationInfo newMatsya = new Matsya();
                weapon = BuildSpread(parent, newMatsya);
                prefix = newMatsya.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "SnipeyBoi":
                //Load generated SnipeyBoi stats onto weapon
                WeaponGenerationInfo newSnipeyBoi = new SnipeyBoi();
                weapon = BuildPrecision(parent, newSnipeyBoi);
                prefix = newSnipeyBoi.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            default:
                return weapon;
        }

        //Apply the newly generated prefix to the weapon name
        Weapon wep = weapon.GetComponent<Weapon>();
        return weapon;
    }

    /// <summary>
    /// Main function to generate a weapon
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GenerateWeapon(Transform parent)
    {
        //parse into a weapon type
        int rngVal = UnityEngine.Random.Range(0, Weapon.WeaponsList.Count);

        //Load the randomly picked weapon's name
        string weaponName = Weapon.WeaponsList[rngVal];

        //Create empty object to load generated weapon into
        GameObject weapon = new GameObject();

       //Create empty string to load generated prefix into
        string prefix = "";

        //Generate a weaon specified by its name
        switch(weaponName)
        {
            case "Jakkaru":
                //Load generated Jakkaru stats onto weapon
                WeaponGenerationInfo newJakkaru = new Jakkaru();
                weapon = BuildAutomatic(parent, newJakkaru);
                prefix = newJakkaru.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Matsya":
                //Load generated Matsya stats onto weapon
                WeaponGenerationInfo newMatsya = new Matsya();
                weapon = BuildSpread(parent, newMatsya);
                prefix = newMatsya.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "SnipeyBoi":
                //Load generated SnipeyBoi stats onto weapon
                WeaponGenerationInfo newSnipeyBoi = new SnipeyBoi();
                weapon = BuildPrecision(parent, newSnipeyBoi);
                prefix = newSnipeyBoi.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            default:
                return weapon;
        }

        //Apply the newly generated prefix to the weapon name
        Weapon wep = weapon.GetComponent<Weapon>();
        wep.FullName = prefix + weaponName;
        wep.Name = weaponName;
        return weapon;
    }

    /// <summary>
    /// This function will generate a specified Automatic Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildAutomatic(Transform parent, WeaponGenerationInfo wgi)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/" + wgi.name);
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(wgi.MinSpread, wgi.MaxSpread);
        automatic.Damage = UnityEngine.Random.Range(wgi.MinDamage, wgi.MaxDamage);
        automatic.StunTime = wgi.StunTime;
        automatic.KnockbackImpulse = wgi.MaxKnockbackImpulse;
        automatic.KnockbackTime = wgi.MaxKnockbackTime; ;
        automatic.ClipSize = UnityEngine.Random.Range(wgi.MinClipSize, wgi.MaxClipSize);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = UnityEngine.Random.Range(automatic.ClipSize * MIN_RNG, automatic.ClipSize * MAX_RNG);
        automatic.RateOfFire = UnityEngine.Random.Range(wgi.MinRateOfFire, wgi.MaxRateOfFire);
        automatic.ReloadTime = UnityEngine.Random.Range(wgi.MinReloadTime, wgi.MaxReloadTime);
        automatic.Range = UnityEngine.Random.Range(wgi.MinRange, wgi.MaxRange);
        automatic.BulletVeloc = UnityEngine.Random.Range(wgi.MinBulletVeloc, wgi.MaxBulletVeloc);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        automatic.Description = wgi.description;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a specified Spread Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildSpread(Transform parent, WeaponGenerationInfo wgi)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/" + wgi.name);
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(wgi.MinSpread, wgi.MaxSpread);
        spread.Damage = UnityEngine.Random.Range(wgi.MinDamage, wgi.MaxDamage);
        spread.ClipSize = UnityEngine.Random.Range(wgi.MinClipSize, wgi.MaxClipSize);
        spread.AmmoInClip = spread.ClipSize;
        spread.StunTime = wgi.StunTime;
        spread.KnockbackImpulse = wgi.MaxKnockbackImpulse;
        spread.KnockbackTime = wgi.MaxKnockbackTime;
        spread.MaxAmmoCapacity = UnityEngine.Random.Range(spread.ClipSize * MIN_RNG, spread.ClipSize * MAX_RNG);
        spread.RateOfFire = UnityEngine.Random.Range(wgi.MinRateOfFire, wgi.MaxRateOfFire);
        spread.ReloadTime = UnityEngine.Random.Range(wgi.MinReloadTime, wgi.MaxReloadTime);
        spread.NumPellets = UnityEngine.Random.Range(wgi.MinNumPellets, wgi.MaxNumPellets);
        spread.Range = UnityEngine.Random.Range(wgi.MinRange, wgi.MaxRange);
        spread.BulletVeloc = UnityEngine.Random.Range(wgi.MinBulletVeloc, wgi.MaxBulletVeloc);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        spread.Description = wgi.description;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a specified Precision Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildPrecision(Transform parent, WeaponGenerationInfo wgi)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/" + wgi.name);
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = wgi.MinSpread;
        precision.Damage = UnityEngine.Random.Range(wgi.MinDamage, wgi.MaxDamage);
        precision.ClipSize = UnityEngine.Random.Range(wgi.MinClipSize, wgi.MaxClipSize);
        precision.AmmoInClip = precision.ClipSize;
        precision.StunTime = wgi.StunTime;
        precision.KnockbackImpulse = wgi.MaxKnockbackImpulse;
        precision.KnockbackTime = wgi.MaxKnockbackTime;
        precision.MaxAmmoCapacity = UnityEngine.Random.Range(precision.ClipSize * MIN_RNG, precision.ClipSize * MAX_RNG);
        precision.RateOfFire = UnityEngine.Random.Range(wgi.MinRateOfFire, wgi.MaxRateOfFire);
        precision.ReloadTime = UnityEngine.Random.Range(wgi.MinReloadTime, wgi.MaxReloadTime);
        precision.Range = UnityEngine.Random.Range(wgi.MinRange, wgi.MaxRange);
        precision.BulletVeloc = UnityEngine.Random.Range(wgi.MinBulletVeloc, wgi.MaxBulletVeloc);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        precision.Description = wgi.description;
        return weaponObject;
    }
}
