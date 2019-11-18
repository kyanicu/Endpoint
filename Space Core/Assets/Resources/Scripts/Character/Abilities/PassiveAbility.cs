using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for an ability meant to be activated automatically by the game depending on the current state 
/// </summary>
public abstract class PassiveAbility : Ability
{
    //List that holds the names of all passive abilities
    public static List<string> PassiveAbilityList = new List<string>()
    {
        "Overclock",
        "ImmortalReloading",
        "PiercingShotAbility"
    };

    //if the owner is an enemy, we do not want passive ability to activate
    protected bool isEnemy;

    /// <summary>
    /// Check to see if the owner is an enemy.
    /// </summary>
    protected void Start()
    {
        if (owner.GetComponent<Enemy>() != null)
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
        }
    }

    public new void resetOwner(Character character)
    {
        if (owner.GetComponent<Enemy>() != null)
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
        }
        base.resetOwner(character);
    }

    /// <summary>
    /// Checks every frame for the ability to be activated based on the activation condition
    /// </summary>
    protected void Update()
    {
        if (!isEnemy && activationCondition)
            Activate();
    }
}
