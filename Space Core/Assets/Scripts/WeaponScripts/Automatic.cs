﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        SpreadFactor = 0;
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

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            Quaternion pelletRotation = transform.rotation;
            pelletRotation.x = 0.0f;
            pelletRotation.y = 0.0f;
            pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
            GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Damage = Damage;
            bulletScript.Range = Range;
            FireTimer = RateOfFire;
        }
        else if (AmmoInClip <= 0 && !IsReloading)
        {
            StartCoroutine(Reload());
        }
    }
}
