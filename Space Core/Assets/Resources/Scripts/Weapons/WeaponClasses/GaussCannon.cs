﻿using System.Collections;
using UnityEngine;

/// <summary>
/// This class holds the behavior for a precision weapon
/// </summary>
public class GaussCannon : Weapon
{
    //line renderer used to show raycast of the precision weapon
    private LineRenderer lineRenderer;

    /// <summary>
    /// Start function to initalize variables
    /// </summary>
    public void Start()
    {
        lineRenderer = transform.Find("Laser").gameObject.GetComponent<LineRenderer>();
        FireLocation = transform.Find("FirePoint").gameObject;
        RotationPoint = transform.parent.transform.parent;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Precision;
    }

    /// <summary>
    /// Override base weapon Update to implement line renderer behavior
    /// </summary>
    new void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(FireLocation.transform.position.x, FireLocation.transform.position.y, FireLocation.transform.position.z));
        bool previous = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D[] hits = Physics2D.RaycastAll(FireLocation.transform.position, FireLocation.transform.right, Range);
        Physics2D.queriesHitTriggers = previous;

        //If we hit another object that does not share the same tag as this object's bullet source then set line position
        //to that object's x and y position
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Terrain" || (hit && hit.collider && (hit.collider.gameObject.tag != BulletSource.ToString()) && (hit.collider.tag != "Bullet")))
                {
                    lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y));
                    break;
                }
                else
                {
                    lineRenderer.SetPosition(1, FireLocation.transform.right * Range);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, FireLocation.transform.right * Range);
        }
        
        
        //If we have fired, set the color of the line to red, otherwise make it yellow
        if (FireTimer > 0)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else
        {
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
        }

        base.Update();
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