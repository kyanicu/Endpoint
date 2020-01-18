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
        "Immortal Reloading",
        "Piercing Shot"
    };

    //if the owner is an enemy, we do not want passive ability to activate
    protected bool isEnemy;

    /// <summary>
    /// Check to see if the owner is an enemy.
    /// </summary>
    protected void Start()
    {
        isEnemy = owner.GetComponent<Enemy>() != null;
    }

    public new void resetOwner(Character character)
    {
        isEnemy = !character.gameObject.CompareTag("Player");
        base.resetOwner(character);
    }

    /// <summary>
    /// Checks every frame for the ability to be activated based on the activation condition
    /// </summary>
    protected void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (!isEnemy && activationCondition)
            Activate();
    }
}
