using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    new void Awake()
    {
        PatrolRange = 3.0f;
        base.Awake();
    }
}
