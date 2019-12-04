﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInfo
{
    public int damage;
    public Vector2 knockbackImpulse;
    public float knockbackTime;
    public float stunTime;
    
    public AttackInfo(int dmg, Vector2 kb, float kbTime, float stun)
    {
        damage = dmg;
        knockbackImpulse = kb;
        knockbackTime = kbTime;
        stunTime = stun;
    }
}
