using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : Ability
{

    void Awake()
    {
        type = Ability.AbilityType.PASSIVE;
    }

    protected abstract bool CheckActivation();

    // Update is called once per frame
    void Update()
    {
        if (CheckActivation())
            Activate();
    }
}
