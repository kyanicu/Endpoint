using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHealthRegen : WheelUpgrade
{
    protected override bool activationCondition => false;

    public override void DeactivateAbility()
    {
        PlayerController.instance.HealthRegenOnPickup = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.HealthRegenOnPickup = true;
    }
}
