using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemy : Enemy
{
    new void Awake()
    {
        PatrolRange = 6.0f;
        base.Awake();
    }
}
