using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : Weapon
{
    public void Start()
    {
        BulletTag = "EnergyBurst";
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
            FireTimer = RateOfFire;
            energyScript.Velocity = BulletVeloc;
            energyScript.Activate();
            return true;
        }
        else
            return false;
    }
}
