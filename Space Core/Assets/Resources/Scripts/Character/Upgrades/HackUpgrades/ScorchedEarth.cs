using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedEarth : WheelUpgrade
{
    protected override bool activationCondition => true;
    public static float DamageOnSwitch = 15.0f;

    public override void DeactivateAbility()
    {
        PlayerController.instance.ScorchedEarthActive = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.ScorchedEarthActive = true;
    }
}
