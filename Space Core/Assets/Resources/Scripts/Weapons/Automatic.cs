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
    public override bool Fire()
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
            bulletScript.KnockbackImpulse = KnockbackImpulse;
            bulletScript.KnockbackTime = KnockbackTime;
            bulletScript.StunTime = StunTime;
            bulletScript.Source = BulletSource;
            bulletScript.Range = Range;
            bulletScript.Velocity = BulletVeloc;
            FireTimer = RateOfFire;
            return true;
        }
        else
        {
            return false;
        }
    }
}
