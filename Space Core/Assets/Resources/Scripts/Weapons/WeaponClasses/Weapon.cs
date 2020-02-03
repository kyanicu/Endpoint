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

    public enum WeaponType { Automatic, Scatter, Precision };

    public string Name { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public bool IsReloading { get; set; }
    public DamageSource BulletSource { get; set; }
    public WeaponType Type { get; set; }
    public int AmmoInClip { get; set; }
    public float SpreadFactor { get; set; }
    public int TotalAmmo { get; set; }
    public int ClipSize { get; set; }
    public int MaxAmmoCapacity { get; set; }
    public int Damage { get; set; }
    public float StunTime { get; set; }
    public float KnockbackImpulse { get; set; }
    public float KnockbackTime { get; set; }
    public float RateOfFire { get; set; }
    public float FireTimer { get; set; }
    public float Range { get; set; }
    public float ReloadTime { get; set; }
    public bool ControlledByPlayer { get; set; }
    public string BulletTag { protected get; set; }
    public GameObject FireLocation { get; set; }
    public Character owner { get; set; }

    public float BulletVeloc
    {
        get { return (ControlledByPlayer) ? _bulletVelocity * playerBulletVelocMod : _bulletVelocity * enemyBulletVelocMod; }
        set { _bulletVelocity = value; }
    }

    protected Transform RotationPoint;
    private float playerBulletVelocMod = 1.5f;
    private float enemyBulletVelocMod = .75f;
    private float _bulletVelocity;


    public Vector2 aimingDirection
    {
        get
        {
            if (RotationPoint == null)
            {
                return transform.right;
            }

            return RotationPoint.transform.right;
        }
    }

    private void Awake()
    {
        owner = transform.root.GetComponent<Character>();
    }

    /// <summary>
    /// Function that must be implemented to control firing behavior
    /// </summary>
    public abstract bool Fire();

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

        //Check that passed character still exists
        if(c != null)
        {
            //Begin reloading loop
            while (TotalAmmo > 0 && AmmoInClip < ClipSize)
            {
                if (c == null) break;

                //Wait until reload timer is up.
                yield return new WaitForSeconds(ReloadTime / ClipSize);

                //Fill mag with remaining ammo
                TotalAmmo--;
                AmmoInClip++;

                //Check if player is reloading to update HUD
                if (ControlledByPlayer)
                {
                    HUDController.instance.UpdateAmmo(c);
                }
            }

            IsReloading = false;
            yield return null;
        }
    }

    /// <summary>
    /// Main update function decrementing fire timer
    /// </summary>
    protected void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

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
