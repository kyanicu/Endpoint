using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : Ability
{

    // Update is called once per frame
    void Update()
    {
        if (activationCondition)
            Activate();
    }
}
