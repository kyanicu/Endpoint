using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for an ability meant to be directly activated by the player or by an AI
/// </summary>
public abstract class ActiveAbility : Ability
{
    //timer that indicates when the timer can be active
    protected float activationTimer;
    //timer for the cooldown of the ability
    public float Cooldown { get; set; }

    //List that holds the names of all active abilities
    public static List<string> ActiveAbilityList = new List<string>()
    {
        "Homing Bullet",
        "EMP Grenade",
    };

    /// <summary>
    /// Allows the powner Character to attempt activation of the ability
    /// </summary>
    /// <returns>If ability was activated</returns>
    public bool AttemptActivation()
    {
        if (activationCondition)
        {
            //if (GetComponent<Player>())
            //{
                HUDController.instance.StartAbilityCooldown(Cooldown);
            //}

            Activate();
            return true;
        }
        else
        {
            return false;
        }
    }

}
