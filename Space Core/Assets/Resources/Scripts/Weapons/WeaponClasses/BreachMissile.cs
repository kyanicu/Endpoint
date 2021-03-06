﻿using UnityEngine;

public class BreachMissile : Weapon
{
    public void Start()
    {
        RotationPoint = transform.parent.transform.parent;
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Heavy;
    }

    public override bool Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            //Get the rocket object from the ObjectPooler
            GameObject rocket = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
            GameObject muzzleFlash = ObjectPooler.instance.SpawnFromPool("BMMuzzleFlash", FireLocation.transform.position, Quaternion.identity);
            //muzzleFlash.transform.Rotate(RotationPoint.rotation.eulerAngles);

            if (rocket != null)
            {
                IsReloading = false;
                AmmoInClip -= 1;
                Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
                pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
                rocket.transform.Rotate(pelletRotation);
                Rocket rocketScript = rocket.GetComponentInChildren<Rocket>();
                if (rocketScript == null)
                {
                    rocket.SetActive(false);
                    return false;
                }
                //Pass AttackInfo to the rocket being fired
                rocketScript.attInfo = new AttackInfo(Damage, Vector2.zero, KnockbackTime, StunTime, BulletSource);
                rocketScript.KnockbackImpulse = KnockbackImpulse;
                FireTimer = RateOfFire;
                rocketScript.Velocity = BulletVeloc;
                rocketScript.Homing = BulletHoming;
                rocketScript.Range = Range;
                rocketScript.Activate();
                audioSource.clip = FireSfx;
                audioSource.Play();
                return true;
            }
            return false;
        }
        else
            return false;
    }
}
