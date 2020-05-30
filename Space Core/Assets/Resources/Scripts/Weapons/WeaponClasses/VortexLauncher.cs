using System.Collections;
using UnityEngine;

public class VortexLauncher : RotaryRepeater
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
            bulletScript.Homing = BulletHoming;
            bulletScript.Source = BulletSource;
            bulletScript.Range = Range;
            bulletScript.Velocity = BulletVeloc;
            FireTimer = RateOfFire;
            bulletScript.Activate();
                
            //TODO: Remove HUD update from Weapon class
            if (ControlledByPlayer)
            {
                HUDController.instance.UpdateAmmo(owner); // Update Weapon Ammo in HUD
            }

            audioSource.clip = FireSfx;
            audioSource.Play();
            yield return new WaitForSeconds(RateOfFire);
        }
        
        // Reload weapon if out of bullets
        if (AmmoInClip == 0)
        {
            Reload(owner);
        }
    }
}
