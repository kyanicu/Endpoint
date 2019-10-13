using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsReloading { get; set; }
    public int AmmoInClip { get; protected set; }
    public float SpreadFactor { get; set; }
    public int TotalAmmo { get; set; }
    public int ClipSize { get; set; }
    public int MaxAmmoCapacity { get; set; }
    public int Damage { get; set; }
    public float RateOfFire { get; set; }
    public float FireTimer { get; set; }
    public float Range { get; set; }
    public float ReloadTime { get; set; }
    protected object ReloadLock = new object();
    protected GameObject FireLocation;
    protected GameObject Bullet;

    public abstract void Fire();

    public abstract IEnumerator Reload();

    protected void Update()
    {
        if (FireTimer >= 0)
        {
            FireTimer -= Time.deltaTime;
        }
    }

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
