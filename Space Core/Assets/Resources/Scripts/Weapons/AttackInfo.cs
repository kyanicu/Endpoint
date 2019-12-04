using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageSource { Player, Enemy, Hazard, Spread }

public struct AttackInfo
{
    public int damage;
    public Vector2 knockbackImpulse;
    public float stunTime;
    public DamageSource damageSource;

    public AttackInfo(int dmg, Vector2 kb, float stun, DamageSource ds)
    {
        damage = dmg;
        knockbackImpulse = kb;
        stunTime = stun;
        damageSource = ds;
    }
}
