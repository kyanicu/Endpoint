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
    public GameObject LaserParticles;
    public GameObject beamStart;
    public GameObject beamEnd;
    public GameObject beam;
    private LineRenderer line;
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
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

    protected void Awake()
    {
        line = beam.GetComponent<LineRenderer>();
    }

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
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
        {
            return;
        }

        base.Update();

        if (state == State.Independent && !Stunned)
        {
            MoveAlongCeiling();
        }
        else if (state == State.ConnectedToBoss && !Stunned)
        {
            Vector3 playerPosition = PlayerController.instance.Character.transform.position;
            Vector3 myPosition = transform.position;
            Vector3 diff = playerPosition - myPosition;
            AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        }
    }

    private bool FireBaseProjectile()
    {
        GameObject bullet = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
        GameObject muzzleFlash = ObjectPooler.instance.SpawnFromPool("RRMuzzleFlash", FireLocation.transform.position, Quaternion.identity);
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
                LaserParticles.SetActive(true);
            }
        }
        if (!initialMove && laserActiveTimer > 0)
        {
            laserActiveTimer -= Time.deltaTime;
            ShootBeamInDir(FireLocation.transform.position, Vector2.down);
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
            LaserParticles.SetActive(false);
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

    private void ShootBeamInDir(Vector2 start, Vector2 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        bool previous = Physics2D.queriesHitTriggers;
        bool hit = false;
        Physics2D.queriesHitTriggers = true;
        Vector2 end = Vector2.zero;
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, dir);
        Physics2D.queriesHitTriggers = previous;
        foreach (RaycastHit2D rayHit in hits)
        {
            GameObject hitObject = rayHit.transform.gameObject;

            if (hitObject.CompareTag("Terrain"))
            {
                end = rayHit.point - (dir.normalized * beamEndOffset);
                hit = true;
                break;
            }
        }

        if (!hit)
        {
            end = new Vector2(FireLocation.transform.position.x, FireLocation.transform.position.y) + (dir * Range);
        }


        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector2.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
