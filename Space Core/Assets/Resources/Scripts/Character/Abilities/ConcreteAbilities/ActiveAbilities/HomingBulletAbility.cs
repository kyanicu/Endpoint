using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability that activates homing bullets on the owner of the ability
/// </summary>
public class HomingBulletAbility : ActiveAbility
{
    //timer for how long the homing bullet will be active for
    private float activeTime;
    //base bullet object
    private GameObject bullet;
    //gameObject that holds the homing bullet resource
    private GameObject homingBullet;
    //activate when activationTimer is less than zero
    protected override bool activationCondition => activationTimer <= 0;

    /// <summary>
    /// Activate method sets the owner's bullet to the homing bullet
    /// </summary>
    protected override void Activate()
    {
        //get the owner's bullet type. Could be different from standard bullet if they have a
        //passive ability that changes their bullet type.
        owner.Weapon.Bullet = homingBullet;
        StartCoroutine(ResetBullets());
    }

    // Set all resources
    void Start()
    {
        bullet = owner.Weapon.Bullet;
        homingBullet = Resources.Load<GameObject>("Prefabs/Weapons/HomingBullet");
        activeTime = 5f;
        activationTimer = 0f;
        Cooldown = 15f;
    }

    // Initialize all of the data needed for the Ability UI.
    private new void Awake()
    {
        base.Awake();
        AbilityName = "Homing Bullet";
        AbilityShortName = "HOME";
        AbilityDescription = "Push RB to fire bullets that guide themselves for a limited time.";
        AbilityImage = Resources.Load<Sprite>("Images/UI/HUD/Character Section/Ability Images/ability-homing-bullets@1x");
    }

    /// <summary>
    /// Update checks the activation timer and updates it
    /// </summary>
    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        //if we failed to get the bullet at start, do it in update
        if (bullet == null)
        {
            bullet = owner.Weapon.Bullet;
        }
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
        owner.Weapon.Bullet = bullet;
        activationTimer = Cooldown;
    }
}
