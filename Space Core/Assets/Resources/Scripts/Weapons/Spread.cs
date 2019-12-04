﻿using System.Collections;
using UnityEngine;

/// <summary>
/// This class holds all information about the behavior of a spread weapon
/// </summary>
public class Spread : Weapon
{
    //number of pellets being launched in each shot.
    public int NumPellets { get; set; }

    /// <summary>
    /// Initalize all components of the Spread weapon
    /// </summary>
    private void Start()
    {
        Bullet = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        //grab the rotation point for the weapon
        RotationPoint = transform.parent.transform.parent;
    }

    /// <summary>
    /// Fire out a bust of pellets in a random spread
    /// </summary>
    public override void Fire()
    {
        // If we have ammo, are not reloading, and fire timer is zero, launch a spread of bullets
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            IsReloading = false;
            AmmoInClip -= 1;
            //pellet rotation will be used for determining the spread of each bullet
            Vector3 pelletRotation;

            //for the number of pellets we are firing, initalize a new bullet, update rotation and lunch it.
            for (int i = 0; i < NumPellets; i++)
            {
                pelletRotation = RotationPoint.rotation.eulerAngles;
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
                bullet.transform.Rotate(pelletRotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Damage = Damage;
                bulletScript.KnockbackImpulse = KnockbackImpulse;
                bulletScript.KnockbackTime = KnockbackTime;
                bulletScript.StunTime = StunTime;
                bulletScript.Source = BulletSource;
                bulletScript.Range = Range;
                if (ControlledByPlayer)
                {
                    bulletScript.Velocity = BulletVeloc * playerBulletVelocMod;
                }
                else
                {
                    bulletScript.Velocity = BulletVeloc;
                }
            }
            FireTimer = RateOfFire;
        }
    }
}
