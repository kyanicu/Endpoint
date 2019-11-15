using System.Collections;
using UnityEngine;

/// <summary>
/// Class that holds all behavior for an automatic weapon
/// </summary>
public class Automatic : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        Bullet = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        RotationPoint = transform.parent.transform.parent;
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
    }

    /// <summary>
    /// The fire function is used to launch a projectile from the tip of the automatic weapon
    /// </summary>
    public override void Fire()
    {
        //if we have ammo, are not reloading, and the timer will let us fire another shot. Fire a bullet
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            IsReloading = false;
            AmmoInClip -= 1;
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
            pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
            GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
            bullet.transform.Rotate(pelletRotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Damage = Damage;
            bulletScript.Source = BulletSource;
            bulletScript.Range = Range;
            bulletScript.Velocity = BulletVeloc;
            FireTimer = RateOfFire;
        }

        //reload if out of ammo
        else if (AmmoInClip <= 0 && !IsReloading)
        {
           Reload();
        }
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
}
