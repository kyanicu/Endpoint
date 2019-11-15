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

    /// <summary>
    /// Dictionary that holds all weapon names and their weapon types
    /// </summary>
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
    public float BulletVeloc { get; set; }
    public float ReloadTime { get; set; }
    public bool ControlledByPlayer { get; set; }
    public GameObject FireLocation { get; set; }
    protected object ReloadLock = new object();
    protected GameObject Bullet;
    protected Transform RotationPoint;

    /// <summary>
    /// Function that must be implemented to control firing behavior
    /// </summary>
    public abstract void Fire();

    public void Reload()
    {
        StartCoroutine(ReloadRoutine());

        //Give enemy back ammo so they don't run out
        if (!ControlledByPlayer)
        {
            TotalAmmo += ClipSize;
        }
    }

    protected abstract IEnumerator ReloadRoutine();

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
