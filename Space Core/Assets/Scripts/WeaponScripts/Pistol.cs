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
        TotalAmmo = 30;
        Range = 100f;
        fireLocation = GameObject.Find("FirePoint");
    }

    public override void Fire()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.fireLocation.transform.position, this.fireLocation.transform.right, Range);
        if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
        {
            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(this.Damage);
        }
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}
