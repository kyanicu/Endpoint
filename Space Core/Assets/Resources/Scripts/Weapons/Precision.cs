using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precision : Weapon
{
    private LineRenderer lineRenderer;

    public void Start()
    {
        Range = 100f;
        Bullet = Resources.Load<GameObject>("WeaponResources/Bullet");
        lineRenderer = transform.Find("Laser").gameObject.GetComponent<LineRenderer>();
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
    }

    new void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit && hit.collider && hit.collider.gameObject.tag != BulletSource.ToString())
        {
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y));
        }
        else
        {
            lineRenderer.SetPosition(1, FireLocation.transform.right * Range);
        }

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
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right);
            //TODO CLEAN UP IF STATEMENT

            foreach (RaycastHit2D hit in hits)
            {
                string tag = hit.transform.gameObject.tag;
                if (hit.transform != null && (tag == "Enemy" || tag == "Player"))
                {
                    if (hit.transform.gameObject.tag != BulletSource.ToString())
                    {
                        hit.transform.gameObject.GetComponent<Character>().TakeDamage(Damage);
                    }
                }
            }
            FireTimer = RateOfFire;
        }
        else if (AmmoInClip <= 0 && !IsReloading)
        {
            StartCoroutine(Reload());
        }
    }
}
