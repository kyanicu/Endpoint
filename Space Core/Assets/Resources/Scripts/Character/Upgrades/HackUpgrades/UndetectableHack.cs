using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndetectableHack : WheelUpgrade
{
    protected override bool activationCondition => true;

    public static float UndetectableTime = 2.0f;

    public override void DeactivateAbility()
    {
        PlayerController.instance.StealthHackActive = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.StealthHackActive = true;
    }
}
