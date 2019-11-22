using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any weapon type
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    /// <summary>
    /// Dictionary that holds all weapon names and their weapon types
    /// </summary>
    public static Dictionary<int, string> WeaponsList = new Dictionary<int, string>()
    {
        { 0, "Matsya" },
        { 1, "Jakkaru" },
        { 2, "SnipeyBoi" },
        //{ 3, "Tributar" },
        //{ 4, "Bestafera" },
        //{ 5, "Thor" },
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
    public float StunTime { get; set; }
    public float KnockbackImpulse { get; set; }
    public float RateOfFire { get; set; }
    public float FireTimer { get; set; }
    public float Range { get; set; }
    public float BulletVeloc { get; set; }
    public float ReloadTime { get; set; }
    public bool ControlledByPlayer { get; set; }
    public GameObject Bullet { get; set; }
    public GameObject FireLocation { get; set; }
    protected object ReloadLock = new object();
    protected Transform RotationPoint;
    protected float playerBulletVelocMod = 1.5f;

    /// <summary>
    /// Function that must be implemented to control firing behavior
    /// </summary>
    public abstract void Fire();

    public void Reload(Character c)
    {
        if (!IsReloading)
        { 
            StartCoroutine(ReloadRoutine(c));

            //Give enemy back ammo so they don't run out
            if (!ControlledByPlayer)
            {
                TotalAmmo += ClipSize;
            }
        }
    }

    /// <summary>
    /// Main coroutine used to reload the weapons
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadRoutine(Character c)
    {
        //if already reloading, return
        if (IsReloading)
        {
            yield return null;
        }

        //if we have max ammo in our clip, return
        if (AmmoInClip == ClipSize)
        {
            yield return null;
        }

        IsReloading = true;

        //Stall reload for reloading with empty mag
        if (AmmoInClip == 0)
        {
            yield return new WaitForSeconds(.5f);
        }

        //Retrieve what inherited memeber of character is doing the reloading
        bool isPlayer = c.gameObject.GetComponent<Player>();
        Player p = null;

        if (isPlayer)
        {
            p = c.gameObject.GetComponent<Player>();
        }

        //Begin reloading loop
        while (TotalAmmo > 0 && AmmoInClip < ClipSize)
        {
            //Wait until reload timer is up.
            yield return new WaitForSeconds(ReloadTime / ClipSize);

            //Fill mag with remaining ammo
            TotalAmmo--;
            AmmoInClip++;

            //Check if player is reloading to update HUD
            if (isPlayer)
            {
                HUDController.instance.UpdateAmmo(p);
            }
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
