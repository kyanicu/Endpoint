using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemy : Enemy
{
    new void Awake()
    {
        PatrolRange = 3.0f;
        base.Awake();
    }
}
