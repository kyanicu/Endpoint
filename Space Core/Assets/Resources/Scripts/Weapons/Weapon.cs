using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Automatic,
        Spread,
        Precision
    }

    public bool IsReloading { get; set; }
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

    public abstract void Fire();

    public IEnumerator Reload()
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

        yield return new WaitForSeconds(ReloadTime);

        lock (ReloadLock)
        {
            if (TotalAmmo > 0)
            {
                if (TotalAmmo + AmmoInClip > ClipSize)
                {
                    if (AmmoInClip > 0)
                    {
                        TotalAmmo -= ClipSize - AmmoInClip;
                    }
                    else
                    {
                        TotalAmmo -= ClipSize;
                    }
                    AmmoInClip = ClipSize;
                }
                else
                {
                    AmmoInClip = TotalAmmo;
                    TotalAmmo = 0;
                }
            }
        }
        HUDController.instance.UpdateAmmo(this);
        IsReloading = false;
        yield return null;
    }

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
