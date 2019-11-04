using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability
{

    void Awake()
    {
        type = Ability.AbilityType.ACTIVE;
    }

}
