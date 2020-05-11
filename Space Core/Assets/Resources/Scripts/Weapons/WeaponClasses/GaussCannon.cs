using System.Collections;
using UnityEngine;

/// <summary>
/// This class holds the behavior for a precision weapon
/// </summary>
public class GaussCannon : Weapon
{
    /// <summary>
    /// Start function to initalize variables
    /// </summary>
    public void Start()
    {
        FireLocation = transform.Find("FirePoint").gameObject;
        RotationPoint = transform.parent.transform.parent;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Precision;
    }

    /// <summary>
    /// Fires a single raycast shot outwards towards the direction the weapon is facing
    /// If the hit collides with an object that doesnt have the same tag as the the bulletsource
    /// then that object takes damage
    /// </summary>
    public override bool Fire()
    {
        // If we have ammo, are not reloading, and fire timer is zero, launch a spread of bullets
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
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

                //pellet rotation will be used for determining the spread of each bullet
                Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                bullet.transform.Rotate(pelletRotation);
                Bullet bulletScript = bullet.GetComponentInChildren<Bullet>();
                bulletScript.Damage = Damage;
                bulletScript.KnockbackImpulse = KnockbackImpulse;
                bulletScript.KnockbackTime = KnockbackTime;
                bulletScript.StunTime = StunTime;
                bulletScript.Source = BulletSource;
                bulletScript.Range = Range;
                bulletScript.Velocity = BulletVeloc;
                FireTimer = RateOfFire;
                bulletScript.Activate();
                audioSource.clip = FireSfx;
                audioSource.Play();
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
