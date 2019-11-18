﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicMovement : Movement
{


    private void OnValidate()
    {
        //Const Values

        if(!GetComponent<CharacterController2D>())
            gameObject.AddComponent<CharacterController2D>();

    }

    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 7;
        runAccel = 40;
        runDecel = 40;
        jumpVelocity = 15;
        gravityScale = 1;
        jumpCancelMinVel = 12;
        jumpCancelVel = 2;
        airAccel = 50;
        airDecel = 25;
        airMax = 9;
        pushForce = 14;
    }

    

}