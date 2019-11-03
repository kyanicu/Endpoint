using System.Collections;
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
    /// Main coroutine used to reload the weapon
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ReloadRoutine()
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

        IsReloading = false;
        yield return null;
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
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;

            //for the number of pellets we are firing, initalize a new bullet, update rotation and lunch it.
            for (int i = 0; i < NumPellets; i++)
            {
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
                bullet.transform.Rotate(pelletRotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Damage = Damage;
                bulletScript.Source = BulletSource;
                bulletScript.Range = Range;
                bulletScript.Velocity = BulletVeloc;
            }
            FireTimer = RateOfFire;
        }

        //reload if out of ammo
        else if (AmmoInClip <= 0 && !IsReloading)
        {
           Reload();
        }
    }
}
