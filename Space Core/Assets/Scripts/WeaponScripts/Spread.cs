using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : Weapon
{
    public int NumPellets { get; set; }

    private void Start()
    {
        SpreadFactor = 0.1f;
        NumPellets = 6;
        Damage = 8;
        ClipSize = 10;
        AmmoInClip = ClipSize;
        MaxAmmoCapacity = 50;
        RateOfFire = .5f;
        ReloadTime = 2.0f;
        TotalAmmo = 30;
        Range = 100f;
        Bullet = Resources.Load<GameObject>("WeaponResources/Bullet");
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
    }

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            Quaternion pelletRotation = transform.rotation;
            for (int i = 0; i < NumPellets; i++)
            {
                pelletRotation.x = 0.0f;
                pelletRotation.y = 0.0f;
                pelletRotation.z = 0.0f;
                //pelletRotation.x += Random.Range(-SpreadFactor, SpreadFactor);
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, pelletRotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Damage = Damage;
                bulletScript.Range = Range;
            }
            FireTimer = RateOfFire;
        }
        else if (AmmoInClip <= 0 && !IsReloading)
        {
            StartCoroutine(Reload());
        }
    }

    public override IEnumerator Reload()
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

        IsReloading = false;
        yield return null;
    }
}
