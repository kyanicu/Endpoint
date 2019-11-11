using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for an ability meant to be activated automatically by the game depending on the current state 
/// </summary>
public abstract class PassiveAbility : Ability
{

    /// <summary>
    /// Checks every frame for the ability to be activated based on the activation condition
    /// </summary>
    void Update()
    {
        if (activationCondition)
            Activate();
    }
}
