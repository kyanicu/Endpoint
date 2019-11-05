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
        Precision
    }

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
    public IEnumerator Reload()
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

        //Wait until reaload timer is up.
        yield return new WaitForSeconds(ReloadTime);

        //lock the reload object so no concurrent reloads happen
        lock (ReloadLock)
        {
            //if our total ammo is above zero
            if (TotalAmmo > 0)
            {
                //if the amount of ammo in the clip plus the ammo size is greater than the clipsize
                if (TotalAmmo + AmmoInClip > ClipSize)
                {
                    //if we already had ammo in our clip, subtract the difference from total ammo
                    if (AmmoInClip > 0)
                    {
                        TotalAmmo -= ClipSize - AmmoInClip;
                    }
                    //otherwise remove clipsize from the ammo pool
                    else
                    {
                        TotalAmmo -= ClipSize;
                    }
                    //reset ammo in clip
                    AmmoInClip = ClipSize;
                }
                //if we are going to run out of total ammo on this reload
                else
                {
                    //set ammo in clip to total ammo and set total ammo to zero
                    AmmoInClip = TotalAmmo;
                    TotalAmmo = 0;
                }
            }
        }

        //update hud
        GameObject parent = transform.parent.gameObject;
        if (parent.tag == "Player")
            HUDController.instance.UpdateAmmo(this);
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
