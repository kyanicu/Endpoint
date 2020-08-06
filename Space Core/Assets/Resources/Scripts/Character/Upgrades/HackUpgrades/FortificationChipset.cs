using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortificationChipset : WheelUpgrade
{
    public static int HealValue = 20;
    protected override bool activationCondition => true;

    public override void DeactivateAbility()
    {
        PlayerController.instance.HealOnHack = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.HealOnHack = true;
    }
}
