using System.Collections;
using UnityEngine;

/// <summary>
/// This class holds the behavior for a precision weapon
/// </summary>
public class Precision : Weapon
{
    //line renderer used to show raycast of the precision weapon
    private LineRenderer lineRenderer;

    /// <summary>
    /// Start function to initalize variables
    /// </summary>
    public void Start()
    {
        Bullet = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        var asdfas = transform.Find("Laser");
        lineRenderer = transform.Find("Laser").gameObject.GetComponent<LineRenderer>();
        FireLocation = transform.Find("FirePoint").gameObject;
        RotationPoint = transform.parent.transform.parent;
        IsReloading = false;
        FireTimer = 0;
    }

    /// <summary>
    /// Override base weapon Update to implement line renderer behavior
    /// </summary>
    new void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right);

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
            IsReloading = false;
            AmmoInClip -= 1;

            //pellet rotation will be used for determining the spread of each bullet
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
            pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
            GameObject bullet = Instantiate(Bullet, FireLocation.transform.position, Quaternion.identity);
            bullet.transform.Rotate(pelletRotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Damage = Damage;
            bulletScript.KnockbackImpulse = KnockbackImpulse;
            bulletScript.StunTime = StunTime;
            bulletScript.Source = BulletSource;
            bulletScript.Range = Range;

            if (ControlledByPlayer)
            {
                bulletScript.Velocity = BulletVeloc * playerBulletVelocMod;
            }
            else
            {
                bulletScript.Velocity = BulletVeloc * enemyBulletVelocMod;
            }

            FireTimer = RateOfFire;
            return true;
        }
        else
        {
            return false;
        }
    }
}
