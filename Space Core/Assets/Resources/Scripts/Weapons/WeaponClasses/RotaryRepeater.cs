using System.Collections;
using UnityEngine;

/// <summary>
/// Class that holds all behavior for an automatic weapon
/// </summary>
public class RotaryRepeater : Weapon
{

    // Start is called before the first frame update
    void Start()
    {
        RotationPoint = transform.parent.transform.parent;
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Automatic;
    }

    /// <summary>
    /// The fire function is used to launch a projectile from the tip of the automatic weapon
    /// </summary>
    public override bool Fire()
    {
        //if we have ammo, are not reloading, and the timer will let us fire another shot. Fire a bullet
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
