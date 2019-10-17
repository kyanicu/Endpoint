﻿using System.Collections;
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
        if (hit)
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y));
            }
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

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            AmmoInClip -= 1;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
            if (hit.transform != null && hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            }
            FireTimer = RateOfFire;
        }
        else if (AmmoInClip <= 0 && !IsReloading)
        {
            StartCoroutine(Reload());
        }
    }
}