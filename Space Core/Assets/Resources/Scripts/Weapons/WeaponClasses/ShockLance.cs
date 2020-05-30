using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockLance : Weapon
{
    public void Start()
    {
        RotationPoint = transform.parent.transform.parent;
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Energy;
    }

    public override bool Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            //Gets the energyBurst objects from the pool (EnergyBurst functionally same as bullets)
            GameObject energyBurst = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
            //muzzleFlash.transform.Rotate(RotationPoint.rotation.eulerAngles);

            IsReloading = false;
            AmmoInClip -= 1;
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
            pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
            energyBurst.transform.Rotate(pelletRotation);
            Bullet energyScript = energyBurst.GetComponentInChildren<Bullet>();
            if (energyScript == null)
            {
                energyScript = energyBurst.GetComponentInChildren<Bullet>();
            }
            energyScript.Damage = Damage;
            energyScript.KnockbackImpulse = KnockbackImpulse;
            energyScript.KnockbackTime = KnockbackTime;
            energyScript.StunTime = StunTime;
            energyScript.Source = BulletSource;
            energyScript.Range = Range;
            energyScript.Homing = BulletHoming;
            FireTimer = RateOfFire;
            energyScript.Velocity = BulletVeloc;
            energyScript.Activate();
            if (audioSource.clip != FireSfx)
            {
                audioSource.clip = FireSfx;
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            return true;
        }
        else
            return false;
    }

    public override bool EndFire()
    {
        if (IsReloading)
        {
            return false;
        }

        if (audioSource.clip == FireSfx)
        {
            audioSource.Stop();
        }
        return true;
    }
}
