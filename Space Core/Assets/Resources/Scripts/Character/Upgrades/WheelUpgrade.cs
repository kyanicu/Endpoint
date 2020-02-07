using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WheelUpgrade : MonoBehaviour
{
    protected abstract bool activationCondition { get;  }
    public abstract void DeactivateAbility();
    public abstract void EnableAbility();
}
