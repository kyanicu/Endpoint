using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannons : BossWeapon
{
    public enum LaserState { ConnectedToBoss, Independent }

    public LaserState state;
    public Transform StartPoint;
    public Transform EndPointLeft;
    public Transform EndPointRight;
    public float LaserActiveTime;
    public float ActiveLaserStunTime;
    public short Direction;
    private float laserActiveTimer;
    private string baseProjectileName;
    private string fullLazerProjectileName;

    public override bool Activate(int behavior)
    {
        switch (behavior)
        {
            case 0:
                return FireBaseProjectile();
            case 1:
                state = LaserState.Independent;
                return true;
        }

        return false;
    }

    protected override void Update()
    {
        base.Update();

        if (state == LaserState.Independent && !Stunned)
        {
            MoveAlongCeiling();
        }
        else if (state == LaserState.ConnectedToBoss && !Stunned)
        {
            //AimWeapon(0);
        }
    }

    private bool FireBaseProjectile()
    {
        GameObject bullet = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
        GameObject muzzleFlash = ObjectPooler.instance.SpawnFromPool("MuzzleFlash", FireLocation.transform.position, Quaternion.identity);
        muzzleFlash.transform.Rotate(transform.rotation.eulerAngles);

        //Check that bullet was loaded after pooler has been populated
        if (bullet != null)
        {
            Vector3 pelletRotation = transform.rotation.eulerAngles;
            bullet.transform.Rotate(pelletRotation);
            Bullet bulletScript = bullet.GetComponentInChildren<Bullet>();
            if (bulletScript == null)
            {
                return false;
            }
            bulletScript.Damage = Damage;
            bulletScript.KnockbackImpulse = KnockBackImpulse;
            bulletScript.KnockbackTime = KnockBackTime;
            bulletScript.StunTime = StunTime;
            bulletScript.Source = DamageSource.Enemy;
            bulletScript.Range = Range;
            bulletScript.Velocity = ProjectileSpeed;
            FireTimer = RateOfFire;
            bulletScript.Activate();
            //audioSource.clip = FireSfx;
            //audioSource.Play();
            return true;
        }
        return false;
    }

    private void MoveAlongCeiling()
    {

    }
}
