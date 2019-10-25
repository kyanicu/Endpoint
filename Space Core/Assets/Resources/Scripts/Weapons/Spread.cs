using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : Weapon
{
    public int NumPellets { get; set; }

    private void Start()
    {
        Range = 100f;
        Bullet = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
    }

    public override void Fire()
    {
        if (IsReloading && ReloadMethod == ReloadType.AllAtOnce) return;
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            Quaternion pelletRotation = transform.rotation;
            for (int i = 0; i < NumPellets; i++)
            {
                pelletRotation.x = FireLocation.transform.rotation.x;
                pelletRotation.y = FireLocation.transform.rotation.y;
                pelletRotation.z = FireLocation.transform.rotation.z;
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
}
