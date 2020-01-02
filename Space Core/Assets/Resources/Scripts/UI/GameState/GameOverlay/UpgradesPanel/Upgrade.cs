using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public int[] DispositionValues { get; private set; }
    public string FullName { get; private set; }
    public string ShortName { get; private set; }
    public string Description { get; private set; }

    /// <summary>
    /// Basic Upgrade Constructor
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="shortName"></param>
    /// <param name="description"></param>
    /// <param name="disValues"></param>
    public Upgrade(string fullName, string shortName, string description, int[] disValues)
    {
        FullName = fullName;
        ShortName = shortName;
        Description = description;
        DispositionValues = disValues;
    }
}
