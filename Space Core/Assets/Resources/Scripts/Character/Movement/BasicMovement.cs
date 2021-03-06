﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicMovement : Movement
{

    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 7;
        runAccel = 40;
        runDecel = 40;
        runBreak = 100;
        jumpVelocity = 18;
        gravityScale = 1;
        jumpCancelMinVel = 12;
        jumpCancelVel = 2;
        airAccel = 50;
        airDecel = 25;
        airMax = 9;
        pushForce = 14;
        mass = 1;
    }

    /// <summary>
    /// Null function override for SpecialAbility()
    /// </summary>
    public override void SpecialAbility()
    {

    }

}
