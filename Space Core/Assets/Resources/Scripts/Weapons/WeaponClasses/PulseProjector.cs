using System.Collections;
using UnityEngine;

/// <summary>
/// This class holds all information about the behavior of a spread weapon
/// </summary>
public class PulseProjector : Weapon
{
    //extra spread specific audio clips
    public AudioClip PumpStart;
    public AudioClip PumpEnd;

    /// <summary>
    /// Initalize all components of the Spread weapon
    /// </summary>
    private void Start()
    {
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        //grab the rotation point for the weapon
        RotationPoint = transform.parent.transform.parent;
        Type = WeaponType.Scatter;
    }

    /// <summary>
    /// Fire out a bust of pellets in a random spread
    /// </summary>
    public override bool Fire()
    {
        // If we have ammo, are not reloading, and fire timer is zero, launch a spread of bullets
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            //Retrieve bullet from pooler
            GameObject bullet = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
            //GameObject muzzleFlash = ObjectPooler.instance.SpawnFromPool("MuzzleFlash", FireLocation.transform.position, Quaternion.identity);
            //muzzleFlash.transform.Rotate(RotationPoint.rotation.eulerAngles);

            //Check that bullet was loaded after pooler has been populated
            if (bullet != null)
            {
                //Remove bullet used to test status of pooler
                bullet.SetActive(false);

                IsReloading = false;
                AmmoInClip -= 1;
                //pellet rotation will be used for determining the spread of each bullet
                Vector3 pelletRotation;                
                pelletRotation = RotationPoint.rotation.eulerAngles;
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                bullet = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.Euler(pelletRotation));
                //bullet.transform.Rotate(pelletRotation);
                Bullet bulletScript = bullet.GetComponentInChildren<Bullet>();
                bulletScript.Damage = Damage;
                bulletScript.KnockbackImpulse = KnockbackImpulse;
                bulletScript.KnockbackTime = KnockbackTime;
                bulletScript.StunTime = StunTime;
                bulletScript.Source = BulletSource;
                bulletScript.Homing = BulletHoming;
                bulletScript.Range = Range;
                bulletScript.Velocity = BulletVeloc;
                bulletScript.Activate();
                FireTimer = RateOfFire;
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

    private IEnumerator PumpEffect()
    {
        yield return new WaitForSeconds(RateOfFire / 2.0f);
        audioSource.clip = PumpStart;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        if (audioSource.clip != FireSfx)
        {
            audioSource.clip = PumpEnd;
            audioSource.Play();
        }   
    }
}
