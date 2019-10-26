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
        //grab the rotation point for the weapon
        RotationPoint = transform.parent.transform.parent;
    }

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
            for (int i = 0; i < NumPellets; i++)
            {
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
                bullet.transform.Rotate(pelletRotation);
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
