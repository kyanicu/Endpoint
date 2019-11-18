using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The abstract class of a character ability
/// </summary>
public abstract class Ability : MonoBehaviour
{
    /// <summary>
    /// The condition required to allow ability activation
    /// </summary>
    protected abstract bool activationCondition { get;  }

    /// <summary>
    /// Reference to the owning character
    /// </summary>
    private Character owner;

    private void Awake()
    {
        // owner = transform.parent.GetComponent<Character>();
    }

    public void resetOwner(Character character)
    {
        owner = transform.parent.GetComponent<Character>();
    }

    // The method that runs the Ability
    protected abstract void Activate();

}
