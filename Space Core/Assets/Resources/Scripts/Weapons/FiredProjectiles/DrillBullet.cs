using System;
using System.Collections.Generic;
using UnityEngine;

public class DrillBullet : Bullet
{
    private float timePerDamageTic = .2f;
    private float baseVelocity;
    private float reducedVelocityFactor = .1f;
    //this originally was a dictionary with an integer as a key to the unique id,
    //but unity isn't good when it comes to detecting on trigger exit when an object
    //destroyed or disabled
    private Dictionary<GameObject, float> encounteredObjectTimers;

    /// <summary>
    /// initialize all base variables
    /// </summary>
    public override void Activate()
    {
        base.Activate();
        baseVelocity = Velocity;
        encounteredObjectTimers = new Dictionary<GameObject, float>();
    }

    /// <summary>
    /// update all timers held in dictionary
    /// </summary>
    public override void Update()
    {
        if (encounteredObjectTimers.Count > 0)
        {
            List<GameObject> keys = new List<GameObject>(encounteredObjectTimers.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == null)
                {
                    encounteredObjectTimers.Remove(keys[i]);
                }
                else
                {
                    encounteredObjectTimers[keys[i]] -= Time.deltaTime;
                }
            }

            if (encounteredObjectTimers.Count == 0)
            {
                Velocity = baseVelocity;
            }
        }

        base.Update();
    }

    /// <summary>
    /// On trigger enter, deal damage, reduce speed, and add timer to the dictionary
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
        {
            Homing = false;
            transform.parent.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                if (!encounteredObjectTimers.ContainsKey(collision.gameObject))
                {
                    encounteredObjectTimers.Add(collision.gameObject, timePerDamageTic);
                }
                DealDamage(collision);
                if (Velocity >= baseVelocity)
                {
                    Velocity = Velocity * reducedVelocityFactor;
                }
            }
        }
    }

    /// <summary>
    /// deal damage to specific game object if its time is less than zero
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                float damageTimer;
                int key = collision.gameObject.GetInstanceID();
                if (encounteredObjectTimers.TryGetValue(collision.gameObject, out damageTimer)
                    && damageTimer <= 0)
                {
                    DealDamage(collision);
                    ObjectPooler.instance.SpawnFromPool("VLHit", gameObject.transform.position, Quaternion.identity);
                    encounteredObjectTimers[collision.gameObject] = timePerDamageTic;
                }
            }
        }
    }

    /// <summary>
    /// Remove timer from dictionary on trigger exit
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                encounteredObjectTimers.Remove(collision.gameObject);
                if (encounteredObjectTimers.Count == 0)
                {
                    Velocity = baseVelocity;
                }
            }
        }
    }
}
