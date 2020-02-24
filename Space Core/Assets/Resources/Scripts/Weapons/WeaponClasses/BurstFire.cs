using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFire : Automatic
{
    /// <summary>
    /// The fire function is used to launch a projectile from the tip of the burst fire weapon
    /// </summary>
    public override bool Fire()
    {
        //if we have ammo, are not reloading, and the timer will let us fire another shot. Fire a bullet
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            StartCoroutine(Shoot());
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Fires a burst of 3 bullets, one by one, and then updates the HUD
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shoot()
    {
        int nBullets = (AmmoInClip < 3) ? AmmoInClip : 3;
        for (int i = 0; i < nBullets; i++)
        {
            //Retrieve bullet from pooler
            GameObject bullet = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
            GameObject muzzleFlash = ObjectPooler.instance.SpawnFromPool("MuzzleFlash", FireLocation.transform.position, Quaternion.identity);
            muzzleFlash.transform.Rotate(RotationPoint.rotation.eulerAngles);

            //Check that bullet was loaded after pooler has been populated
            if (bullet != null)
            {
                IsReloading = false;
                AmmoInClip -= 1;
                Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                bullet.transform.Rotate(pelletRotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript == null)
                {
                    bulletScript = bullet.GetComponentInChildren<Bullet>();
                }
                bulletScript.Damage = Damage;
                bulletScript.KnockbackImpulse = KnockbackImpulse;
                bulletScript.KnockbackTime = KnockbackTime;
                bulletScript.StunTime = StunTime;
                bulletScript.Source = BulletSource;
                bulletScript.Range = Range;
                bulletScript.Velocity = BulletVeloc;
                FireTimer = RateOfFire * nBullets;
                bulletScript.Activate();
                HUDController.instance.UpdateAmmo(owner); // Update Weapon Ammo in HUD
                yield return new WaitForSeconds(RateOfFire);
            }
        }
        // Reload weapon if out of bullets
        if (AmmoInClip == 0)
        {
            Reload(owner);
        }
    }
}
