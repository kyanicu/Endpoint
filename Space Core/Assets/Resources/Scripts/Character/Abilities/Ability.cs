using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The abstract class of a character ability
/// </summary>
public abstract class Ability : MonoBehaviour
{
    /// <summary>
    /// The condition required to allow ability activation
    /// </summary>
    protected abstract bool activationCondition { get;  }

    public string AbilityName;
    public string AbilityShortName;
    public Sprite AbilityImage;
    public string AbilityDescription;

    /// <summary>
    /// Reference to the owning character
    /// </summary>
    protected Character owner;

    protected void Awake()
    {
        owner = transform.GetComponent<Character>();
    }

    public void resetOwner(Character character)
    {
        owner = character;
    }

    // The method that runs the Ability
    protected abstract void Activate();

}
