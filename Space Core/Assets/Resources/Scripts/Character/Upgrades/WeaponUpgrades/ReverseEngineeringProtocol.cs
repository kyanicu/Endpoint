using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseEngineeringProtocol : WheelUpgrade
{
    protected override bool activationCondition => true;
    public static float AmmoRetentionMod = 0.2f;

    public override void DeactivateAbility()
    {
        PlayerController.instance.ReverseEngineeringProtocolActive = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.ReverseEngineeringProtocolActive = true;
    }
}
