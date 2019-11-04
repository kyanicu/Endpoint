using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public enum AbilityType { PASSIVE, ACTIVE };

    public AbilityType type { get; protected set; }

    public abstract void Activate();

}
