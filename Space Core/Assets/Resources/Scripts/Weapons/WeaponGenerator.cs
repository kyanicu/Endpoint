using System;
using UnityEngine;

#region Weapon Namespaces
using Jakkaru = WeaponGenerationInfo.Jakkaru;
using Matsya = WeaponGenerationInfo.Matsya;
using SnipeyBoi = WeaponGenerationInfo.SnipeyBoi;
//using Thor = WeaponGenerationInfo.Thor;
//using Bestafera = WeaponGenerationInfo.Bestafera;
//using Tributar = WeaponGenerationInfo.Tributar;
#endregion

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
                weapon = BuildJakkaru(parent);
                prefix = Jakkaru.GenerateAutomaticName(weapon.GetComponent<Weapon>());
                break;

            /*case "Thor":
                //Load generated Thor stats onto weapon
                weapon = BuildThor(parent);
                prefix = Thor.GenerateAutomaticName(weapon.GetComponent<Weapon>());
                break;
                */

            case "Matsya":
                //Load generated Matsya stats onto weapon
                weapon = BuildMatsya(parent);
                prefix = Matsya.GenerateSpreadName(weapon.GetComponent<Weapon>());
                break;

            /*case "Bestafera":
                //Load generated Bestafera stats onto weapon
                weapon = BuilBestafera(parent);
                prefix = Matsya.GenerateSpreadName(weapon.GetComponent<Weapon>());
                break;*/

            case "SnipeyBoi":
                //Load generated SnipeyBoi stats onto weapon
                weapon = BuildSnipeyBoi(parent);
                prefix = SnipeyBoi.GeneratePrecisionName(weapon.GetComponent<Weapon>());
                break;

            /*case "Tributar":
                //Load generated Tributar stats onto weapon
                weapon = BuildTributar(parent);
                prefix = SnipeyBoi.GeneratePrecisionName(weapon.GetComponent<Weapon>());
                break;*/

            default:
                return weapon;
        }

        //Apply the newly generated prefix to the weapon name
        Weapon wep = weapon.GetComponent<Weapon>();
        wep.Name = prefix + weaponName;
        return weapon;
    }

    #region Automatic Weapons
    /// <summary>
    /// This function will generate a random Jakkaru
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildJakkaru(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Automatic");
        //GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Jakkaru");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(Jakkaru.MinSpread, Jakkaru.MaxSpread);
        automatic.Damage = UnityEngine.Random.Range(Jakkaru.MinDamage, Jakkaru.MaxDamage);
        automatic.ClipSize = UnityEngine.Random.Range(Jakkaru.MinClipSize, Jakkaru.MaxClipSize);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = UnityEngine.Random.Range(automatic.ClipSize * MIN_RNG, automatic.ClipSize * MAX_RNG);
        automatic.RateOfFire = UnityEngine.Random.Range(Jakkaru.MinRateOfFire, Jakkaru.MaxRateOfFire);
        automatic.ReloadTime = UnityEngine.Random.Range(Jakkaru.MinReloadTime, Jakkaru.MaxReloadTime);
        automatic.Range = UnityEngine.Random.Range(Jakkaru.MinRange, Jakkaru.MaxRange);
        automatic.BulletVeloc = UnityEngine.Random.Range(Jakkaru.MinBulletVeloc, Jakkaru.MaxBulletVeloc);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a random Thor
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    /*private static GameObject BuildThor(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Thor");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Automatic automatic = weaponObject.GetComponent<Automatic>();
        automatic.SpreadFactor = UnityEngine.Random.Range(Thor.MinSpread, Thor.MaxSpread);
        automatic.Damage = UnityEngine.Random.Range(Thor.MinDamage, Thor.MaxDamage);
        automatic.ClipSize = UnityEngine.Random.Range(Thor.MinClipSize, Thor.MaxClipSize);
        automatic.AmmoInClip = automatic.ClipSize;
        automatic.MaxAmmoCapacity = UnityEngine.Random.Range(automatic.ClipSize * MIN_RNG, automatic.ClipSize * MAX_RNG);
        automatic.RateOfFire = UnityEngine.Random.Range(Thor.MinRateOfFire, Thor.MaxRateOfFire);
        automatic.ReloadTime = UnityEngine.Random.Range(Thor.MinReloadTime, Thor.MaxReloadTime);
        automatic.Range = UnityEngine.Random.Range(Thor.MinRange, Thor.MaxRange);
        automatic.BulletVeloc = UnityEngine.Random.Range(Thor.MinBulletVeloc, Thor.MaxBulletVeloc);
        automatic.TotalAmmo = automatic.MaxAmmoCapacity;
        return weaponObject;
    }*/
    #endregion

    #region Scatter Weapons
    /// <summary>
    /// This function will generate a random Matsya
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildMatsya(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Spread");
        //GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Matsya");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(Matsya.MinSpread, Matsya.MaxSpread);
        spread.Damage = UnityEngine.Random.Range(Matsya.MinDamage, Matsya.MaxDamage);
        spread.ClipSize = UnityEngine.Random.Range(Matsya.MinClipSize, Matsya.MaxClipSize);
        spread.AmmoInClip = spread.ClipSize;
        spread.MaxAmmoCapacity = UnityEngine.Random.Range(spread.ClipSize * MIN_RNG, spread.ClipSize * MAX_RNG);
        spread.RateOfFire = UnityEngine.Random.Range(Matsya.MinRateOfFire, Matsya.MaxRateOfFire);
        spread.ReloadTime = UnityEngine.Random.Range(Matsya.MinReloadTime, Matsya.MaxReloadTime);
        spread.NumPellets = UnityEngine.Random.Range(Matsya.MinNumPellets, Matsya.MaxNumPellets);
        spread.Range = UnityEngine.Random.Range(Matsya.MinRange, Matsya.MaxRange);
        spread.BulletVeloc = UnityEngine.Random.Range(Matsya.MinBulletVeloc, Matsya.MaxBulletVeloc);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a random Bestafera
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    /*private static GameObject BuildBestafera(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Bestafera");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Spread spread = weaponObject.GetComponent<Spread>();
        spread.SpreadFactor = UnityEngine.Random.Range(Bestafera.MinSpread, Bestafera.MaxSpread);
        spread.Damage = UnityEngine.Random.Range(Matsya.Bestafera, Bestafera.MaxDamage);
        spread.ClipSize = UnityEngine.Random.Range(Bestafera.MinClipSize, Bestafera.MaxClipSize);
        spread.AmmoInClip = spread.ClipSize;
        spread.MaxAmmoCapacity = UnityEngine.Random.Range(spread.ClipSize * MIN_RNG, spread.ClipSize * MAX_RNG);
        spread.RateOfFire = UnityEngine.Random.Range(Bestafera.MinRateOfFire, Bestafera.MaxRateOfFire);
        spread.ReloadTime = UnityEngine.Random.Range(Bestafera.MinReloadTime, Bestafera.MaxReloadTime);
        spread.NumPellets = UnityEngine.Random.Range(Bestafera.MinNumPellets, Bestafera.MaxNumPellets);
        spread.Range = UnityEngine.Random.Range(Bestafera.MinRange, Bestafera.MaxRange);
        spread.BulletVeloc = UnityEngine.Random.Range(Bestafera.MinBulletVeloc, Bestafera.MaxBulletVeloc);
        spread.TotalAmmo = spread.MaxAmmoCapacity;
        return weaponObject;
    }*/
    #endregion

    #region Precision Weapons
    /// <summary>
    /// This function will generate a random SnipeyBoi
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildSnipeyBoi(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Precision");
        //GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/SnipeyBoi");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = SnipeyBoi.MinSpread;
        precision.Damage = UnityEngine.Random.Range(SnipeyBoi.MinDamage, SnipeyBoi.MaxDamage);
        precision.ClipSize = UnityEngine.Random.Range(SnipeyBoi.MinClipSize, SnipeyBoi.MaxClipSize);
        precision.AmmoInClip = precision.ClipSize;
        precision.MaxAmmoCapacity = UnityEngine.Random.Range(precision.ClipSize * MIN_RNG, precision.ClipSize * MAX_RNG);
        precision.RateOfFire = UnityEngine.Random.Range(SnipeyBoi.MinRateOfFire, SnipeyBoi.MaxRateOfFire);
        precision.ReloadTime = UnityEngine.Random.Range(SnipeyBoi.MinReloadTime, SnipeyBoi.MaxReloadTime);
        precision.Range = UnityEngine.Random.Range(SnipeyBoi.MinRange, SnipeyBoi.MaxRange);
        precision.BulletVeloc = UnityEngine.Random.Range(SnipeyBoi.MinBulletVeloc, SnipeyBoi.MaxBulletVeloc);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        return weaponObject;
    }

    /// <summary>
    /// This function will generate a random Tributar
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    /*private static GameObject BuildTributar(Transform parent)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/Tributar");
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Precision precision = weaponObject.GetComponent<Precision>();
        precision.SpreadFactor = Tributar.MinSpread;
        precision.Damage = UnityEngine.Random.Range(Tributar.MinDamage, Tributar.MaxDamage);
        precision.ClipSize = UnityEngine.Random.Range(Tributar.MinClipSize, Tributar.MaxClipSize);
        precision.AmmoInClip = precision.ClipSize;
        precision.MaxAmmoCapacity = UnityEngine.Random.Range(precision.ClipSize * MIN_RNG, precision.ClipSize * MAX_RNG);
        precision.RateOfFire = UnityEngine.Random.Range(Tributar.MinRateOfFire, Tributar.MaxRateOfFire);
        precision.ReloadTime = UnityEngine.Random.Range(Tributar.MinReloadTime, Tributar.MaxReloadTime);
        precision.Range = UnityEngine.Random.Range(Tributar.MinRange, Tributar.MaxRange);
        precision.BulletVeloc = UnityEngine.Random.Range(Tributar.MinBulletVeloc, Tributar.MaxBulletVeloc);
        precision.TotalAmmo = precision.MaxAmmoCapacity;
        return weaponObject;
    }*/
    #endregion 
}
