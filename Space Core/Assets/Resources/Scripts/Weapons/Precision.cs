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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);

        //If we hit another object that does not share the same tag as this object's bullet source then set line position
        //to that object's x and y position
        if (hit && hit.collider && hit.collider.gameObject.tag != BulletSource.ToString())
        {
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y));
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
    public override void Fire()
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
            bulletScript.Source = BulletSource;
            bulletScript.Range = Range;
            bulletScript.Velocity = BulletVeloc;
            FireTimer = RateOfFire;
        }

        //reload if out of ammo
        else if (AmmoInClip <= 0 && !IsReloading)
        {
           Reload();
        }
    }

    /// <summary>
    /// Main coroutine used to reload the weapon
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ReloadRoutine()
    {
        //if already reloading, return
        if (IsReloading)
        {
            yield return null;
        }

        //if we have max ammo in our clip, return
        if (AmmoInClip == ClipSize)
        {
            yield return null;
        }

        IsReloading = true;

        //Wait until reaload timer is up.
        yield return new WaitForSeconds(ReloadTime);

        //lock the reload object so no concurrent reloads happen
        lock (ReloadLock)
        {
            //if our total ammo is above zero
            if (TotalAmmo > 0)
            {
                //if the amount of ammo in the clip plus the ammo size is greater than the clipsize
                if (TotalAmmo + AmmoInClip > ClipSize)
                {
                    //if we already had ammo in our clip, subtract the difference from total ammo
                    if (AmmoInClip > 0)
                    {
                        TotalAmmo -= ClipSize - AmmoInClip;
                    }
                    //otherwise remove clipsize from the ammo pool
                    else
                    {
                        TotalAmmo -= ClipSize;
                    }
                    //reset ammo in clip
                    AmmoInClip = ClipSize;
                }
                //if we are going to run out of total ammo on this reload
                else
                {
                    //set ammo in clip to total ammo and set total ammo to zero
                    AmmoInClip = TotalAmmo;
                    TotalAmmo = 0;
                }
            }
        }

        IsReloading = false;
        yield return null;
    }
}
