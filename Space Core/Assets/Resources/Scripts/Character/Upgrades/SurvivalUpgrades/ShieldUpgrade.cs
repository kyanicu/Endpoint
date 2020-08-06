using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : WheelUpgrade
{
    public int ShieldMax { get; set; }

    protected override bool activationCondition => true;

    private void Start()
    {
        ShieldMax = 3;
    }

    public override void DeactivateAbility()
    {
        PlayerController.instance.ShieldMax = 0;
        PlayerController.instance.Shield = 0;
        enabled = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.ShieldMax = ShieldMax;
        PlayerController.instance.Shield = ShieldMax;
    }
}
