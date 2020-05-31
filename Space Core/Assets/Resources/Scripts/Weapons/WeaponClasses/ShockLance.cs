using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockLance : Weapon
{
    public GameObject LaserParticles;
    public GameObject beamStart;
    public GameObject beamEnd;
    public GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture


    public void Start()
    {
        LaserParticles.SetActive(false);
        line = beam.GetComponent<LineRenderer>();
        RotationPoint = transform.parent.transform.parent;
        FireLocation = transform.Find("FirePoint").gameObject;
        IsReloading = false;
        FireTimer = 0;
        Type = WeaponType.Energy;
    }

    public override bool Fire()
    {
        if (AmmoInClip > 0 && !IsReloading && FireTimer < 0)
        {
            //Gets the energyBurst objects from the pool (EnergyBurst functionally same as bullets)
            GameObject energyBurst = ObjectPooler.instance.SpawnFromPool(BulletTag, FireLocation.transform.position, Quaternion.identity);
            //muzzleFlash.transform.Rotate(RotationPoint.rotation.eulerAngles);

            IsReloading = false;
            AmmoInClip -= 1;
            Vector3 pelletRotation = RotationPoint.rotation.eulerAngles;
            pelletRotation.z += Random.Range(-SpreadFactor, SpreadFactor);
            energyBurst.transform.Rotate(pelletRotation);
            Bullet energyScript = energyBurst.GetComponentInChildren<Bullet>();
            if (energyScript == null)
            {
                energyScript = energyBurst.GetComponentInChildren<Bullet>();
            }
            energyScript.Damage = Damage;
            energyScript.KnockbackImpulse = KnockbackImpulse;
            energyScript.KnockbackTime = KnockbackTime;
            energyScript.StunTime = StunTime;
            energyScript.Source = BulletSource;
            energyScript.Range = Range;
            energyScript.Homing = BulletHoming;
            FireTimer = RateOfFire;
            energyScript.Velocity = BulletVeloc;
            energyScript.Activate();
            if (audioSource.clip != FireSfx)
            {
                audioSource.clip = FireSfx;
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (!LaserParticles.activeSelf)
            {
                LaserParticles.SetActive(true);
            }
            ShootBeamInDir(FireLocation.transform.position, FireLocation.transform.right);

            return true;
        }
        else
            return false;
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, dir, Range);
        Physics2D.queriesHitTriggers = previous;
        
        string compareTag;
        if (owner.CompareTag("Player"))
        {
            compareTag = "Enemy";
        }
        else
        {
            compareTag = "Player";
        }

        foreach (RaycastHit2D rayHit in hits)
        {
            GameObject hitObject = rayHit.transform.gameObject;
            
            if (hitObject.CompareTag(compareTag) || hitObject.CompareTag("Terrain"))
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

    public override bool EndFire()
    {
        if (LaserParticles.activeSelf)
        {
            LaserParticles.SetActive(false);
        }

        if (audioSource.clip == FireSfx)
        {
            audioSource.Stop();
        }
        return true;
    }
}
