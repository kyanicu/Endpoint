using System.Collections;
using UnityEngine;

/// <summary>
/// Ability that activates homing bullets on the owner of the ability
/// </summary>
public class HomingBulletAbility : ActiveAbility
{
    //timer for how long the homing bullet will be active for
    private float activeTime;
    //activate when activationTimer is less than zero
    protected override bool activationCondition => activationTimer <= 0;

    /// <summary>
    /// Activate method sets the owner's bullet to the homing bullet
    /// </summary>
    protected override void Activate()
    {
        //get the owner's bullet type. Could be different from standard bullet if they have a
        //passive ability that changes their bullet type.
        owner.Weapon.BulletHoming = true;
        StartCoroutine(ResetBullets());
    }

    // Set all resources
    void Start()
    {
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
    /// Coroutine that will wait until the active time is up then set the bullets back to normal.
    /// </summary>
    IEnumerator ResetBullets()
    {
        yield return new WaitForSeconds(activeTime);
        owner.Weapon.BulletHoming = false;
        activationTimer = Cooldown;
    }
}
