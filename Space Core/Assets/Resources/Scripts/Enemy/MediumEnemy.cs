using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemy : Enemy
{
    new void Awake()
    {
        PatrolRange = 4.0f;
        base.Awake();
    }
}
