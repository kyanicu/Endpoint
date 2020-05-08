using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannons : BossWeapon
{
    public State state;
    public Transform StartPoint;
    public Transform EndPointLeft;
    public Transform EndPointRight;
    public GameObject LaserBeam;
    public float LaserActiveTime;
    public float ActiveLaserStunTime;
    public float MoveSpeed;
    public short Direction;
    private bool MoveRight;
    private float laserActiveTimer;
    private string baseProjectileName;
    private string fullLazerProjectileName;
    private bool initialMove;
    private const float INITIALMOVEFACTOR = 2.0f;

    public override bool Activate(int behavior)
    {
        switch (behavior)
        {
            case 0:
                return FireBaseProjectile();
            case 1:
                transform.parent = null;
                transform.localRotation = Quaternion.Euler(90, -90, 0);
                state = State.Independent;
                initialMove = true;
                MoveRight = true;
                laserActiveTimer = LaserActiveTime;
                return true;
        }

        return false;
    }

    protected override void Update()
    {
        base.Update();

        if (state == State.Independent && !Stunned)
        {
            MoveAlongCeiling();
        }
        else if (state == State.ConnectedToBoss && !Stunned)
        {
            AimWeapon(0);
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
        float step = Time.deltaTime * MoveSpeed;
        if (initialMove)
        {
            step *= INITIALMOVEFACTOR;
            transform.position = Vector2.MoveTowards(
                                    transform.position,
                                    EndPointLeft.position,
                                    step);
            if (transform.position.x == EndPointLeft.position.x && transform.position.y == EndPointLeft.position.y)
            {
                initialMove = false;
                LaserBeam.SetActive(true);
            }
        }
        if (!initialMove && laserActiveTimer > 0)
        {
            laserActiveTimer -= Time.deltaTime;
            if (MoveRight)
            {
                transform.position = Vector2.MoveTowards(
                                        transform.position,
                                        EndPointRight.position,
                                        step);
                if ((Vector2) transform.position == (Vector2) EndPointRight.position)
                {
                    MoveRight = false;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                                        transform.position,
                                        EndPointLeft.position,
                                        step);

                if ((Vector2) transform.position == (Vector2) EndPointLeft.position)
                {
                    MoveRight = true;
                }
            }
        }
        else if (!initialMove && laserActiveTimer < 0)
        {
            LaserBeam.SetActive(false);
            transform.position = Vector2.MoveTowards(
                                        transform.position,
                                        AttachedBossLocation.position,
                                        step * INITIALMOVEFACTOR);
            if (transform.position.x - AttachedBossLocation.position.x < 1.0f && transform.position.y - AttachedBossLocation.position.y < 1.0f)
            {
                transform.parent = AttachedBossLocation;
                transform.position = AttachedBossLocation.position;
                state = State.ConnectedToBoss;
            }
        }
    }
}
