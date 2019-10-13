using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        Damage = 8;
        ClipSize = 10;
        AmmoInClip = ClipSize;
        MaxAmmoCapacity = 50;
        RateOfFire = .1f;
        ReloadTime = 1.5f;
        TotalAmmo = 30;
        Range = 100f;
        Bullet = Resources.Load<GameObject>("WeaponResources/Bullet");
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
    }

    private void Update()
    {
        if (FireTimer >= 0)
        {
            FireTimer -= Time.deltaTime;
        }
    }

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Damage = Damage;
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

        lock(ReloadLock)
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
