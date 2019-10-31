using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any weapon type
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    //Enum for the different weapon types
    public enum WeaponType
    {
        Automatic,
        Spread,
        Precision,
        //Heavy,
    }

    public static Dictionary<int, Tuple<string, WeaponType>> WeaponsList = new Dictionary<int, Tuple<string, WeaponType>>()
    {
        { 0, Tuple.Create("Okamoto", WeaponType.Spread) },
        { 1, Tuple.Create("Thor", WeaponType.Automatic) },
        { 2, Tuple.Create("Tributar", WeaponType.Precision) },
        { 3, Tuple.Create("Bestafera", WeaponType.Spread) },
        //{ "Korvus", WeaponType.Heavy },
        //{ "Tsar Tsarevich", WeaponType.Heavy },
    };
    
    public string Name { get; set; }
    public bool IsReloading { get; set; }
    public Bullet.BulletSource BulletSource { get; set; }
    public int AmmoInClip { get; set; }
    public float SpreadFactor { get; set; }
    public int TotalAmmo { get; set; }
    public int ClipSize { get; set; }
    public int MaxAmmoCapacity { get; set; }
    public int Damage { get; set; }
    public float RateOfFire { get; set; }
    public float FireTimer { get; set; }
    public float Range { get; set; }
    public float ReloadTime { get; set; }
    public GameObject FireLocation { get; set; }
    protected object ReloadLock = new object();
    protected GameObject Bullet;
    protected Transform RotationPoint;

    /// <summary>
    /// Function that must be implemented to control firing behavior
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Main coroutine used to reload the weapon
    /// </summary>
    /// <returns></returns>
    public void Reload()
    {
        StartCoroutine(ReloadIndividual());
    }

    public IEnumerator ReloadIndividual()
    {
        if (IsReloading)
        {
            yield return null;
        }

        if (AmmoInClip == ClipSize)
        {
            yield return null;
        }

        IsReloading = true;

        if (AmmoInClip == 0)
        {
            yield return new WaitForSeconds(ReloadTime / 4);
        }

        while (TotalAmmo > 0 && AmmoInClip < ClipSize && IsReloading)
        {
            TotalAmmo--;
            AmmoInClip++;
            HUDController.instance.UpdateAmmo(this);
            yield return new WaitForSeconds(ReloadTime / ClipSize);
        }
        IsReloading = false;
        yield return null;
    }


    /// <summary>
    /// Main update function decrementing fire timer
    /// </summary>
    protected void Update()
    {
        if (FireTimer >= 0)
        {
            FireTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Add ammo to the total ammo pool
    /// </summary>
    /// <param name="num">number of bullets to add</param>
    public void AddAmmo(int num)
    {
        if (TotalAmmo + num >= MaxAmmoCapacity)
        {
            TotalAmmo = MaxAmmoCapacity;
        }
        else
        {
            TotalAmmo += num;
        }
    }
}
