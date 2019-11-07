using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability
{

    public bool AttemptActivation()
    {
        if (activationCondition)
        {
            Activate();
            return true;
        }
        else
        {
            return false;
        }
    }

}
