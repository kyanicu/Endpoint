using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        Damage = 8;
        ClipSize = 10;
        AmmoInClip = ClipSize;
        MaxAmmoCapacity = 50;
        RateOfFire = .5f;
        ReloadTime = 1.5f;
        TotalAmmo = 30;
        Range = 100f;
        fireLocation = GameObject.Find("FirePoint");
        GameObject particleSystem = GameObject.Find("FireEffect");
        fireEffect = particleSystem.GetComponent<ParticleSystem>();
        IsReloading = false;
    }

    public override void Fire()
    {
        if (AmmoInClip > 0 && !IsReloading)
        {
            fireEffect.Play();
            AmmoInClip -= 1;
            RaycastHit2D hit = Physics2D.Raycast(this.fireLocation.transform.position, this.fireLocation.transform.right, Range);
            if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(this.Damage);
            }
        }
        else if (AmmoInClip <= 0 && !IsReloading)
        {
            StartCoroutine(Reload());
        }
    }

    public override IEnumerator Reload()
    {
        if (IsReloading)
        {
            yield return null;
        }

        if (AmmoInClip == ClipSize)
        {
            yield return null;
        }

        IsReloading = true;

        yield return new WaitForSeconds(ReloadTime);

        lock(ReloadLock)
        {
            if (TotalAmmo > 0)
            {
                if (TotalAmmo + AmmoInClip > ClipSize)
                {
                    if (AmmoInClip > 0)
                    {
                        TotalAmmo -= ClipSize - AmmoInClip;
                    }
                    else
                    {
                        TotalAmmo -= ClipSize;
                    }
                    AmmoInClip = ClipSize;
                }
                else
                {
                    AmmoInClip = TotalAmmo;
                    TotalAmmo = 0;
                }
            }
        }

        IsReloading = false;
        yield return null;
    }
}
