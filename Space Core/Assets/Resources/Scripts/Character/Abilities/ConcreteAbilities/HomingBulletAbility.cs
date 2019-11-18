﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability that activates homing bullets on the owner of the ability
/// </summary>
public class HomingBulletAbility : ActiveAbility
{
    //timer that indicates when the timer can be active
    private float activationTimer;
    //timer for the cooldown of the ability
    private float cooldown;
    //timer for how long the homing bullet will be active for
    private float activeTime;
    //base bullet object
    private GameObject bullet;
    //gameObject that holds the homing bullet resource
    private GameObject homingBullet;
    //activate when activationTimer is less than zero
    protected override bool activationCondition => activationTimer <= 0;

    /// <summary>
    /// Activate method sets the player's bullet to the homing bullet
    /// </summary>
    protected override void Activate()
    {
        Player.instance.Weapon.Bullet = homingBullet;
        StartCoroutine(ResetBullets());
    }

    // Set all resources
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        homingBullet = Resources.Load<GameObject>("Prefabs/Weapons/HomingBullet");
        activeTime = 5f;
        activationTimer = 0f;
        cooldown = 15f;
    }

    /// <summary>
    /// Update checks the activation timer and updates it
    /// </summary>
    void Update()
    {
        if (activationTimer > 0)
        {
            activationTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Coroutine that will wait until the active time is up then set the bullets back to normal.
    /// </summary>
    IEnumerator ResetBullets()
    {
        yield return new WaitForSeconds(activeTime);
        Player.instance.Weapon.Bullet = bullet;
    }
}
