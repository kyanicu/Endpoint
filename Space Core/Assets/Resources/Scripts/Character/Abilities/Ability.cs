using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected abstract bool activationCondition { get;  }

    private Character owner;

    private void Awake()
    {
        owner = GetComponent<Character>();
    }

    public void resetOwner(Character character)
    {
        owner = GetComponent<Character>();
    }

    protected abstract void Activate();

}
