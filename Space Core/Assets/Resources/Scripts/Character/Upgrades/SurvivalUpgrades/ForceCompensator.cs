using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCompensator : WheelUpgrade
{
    protected override bool activationCondition => true;

    public override void DeactivateAbility()
    {
        PlayerController.instance.ForceCompensatorActive = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.ForceCompensatorActive = true;
    }
}
