using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : Enemy
{
    new void Awake()
    {
        PatrolRange = 8.0f;
        base.Awake();
    }
}
