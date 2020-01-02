using System.Collections.Generic;
using UnityEngine;

public class Paradigm
{
    private static string[] greekAlphabet = { "ALPHA","BETA","GAMMA","DELTA","EPSILON","ZETA","ETA",
                                              "THETA","IOTA","KAPPA", "LAMBDA","MU","NU","XI","OMICRON",
                                              "PI","RHO","SIGMA","TAU","UPSILON","PHI","CHI","PSI","OMEGA"
                                            };
    public string Name { get; private set; }
    public List<int> Branches = new List<int>{ -1, -1, -1, -1, -1 };
    public const int branchCount = 12;
    public const int maxActiveBranches = 5;
    
    /// <summary>
    /// Standard Paradigm generator
    /// </summary>
    public Paradigm()
    {
        for (int x = 0; x < Branches.Count; x++)
        {
            int rngNum = Random.Range(0, branchCount);
            //Loop until unique branch is selected
            while (Branches.Contains(rngNum))
            {
                rngNum = Random.Range(0, branchCount);
            }
            Branches[x] = rngNum;
        }

        //Generate a paradigm name
        int index = Random.Range(0, greekAlphabet.Length);
        string prefix = greekAlphabet[index];
        index = Random.Range(0, 10);
        Name = $"{prefix}-{index}";

        //Keep generating names until a unique one is produced
        while(!checkIfValidName(Name))
        {
            index = Random.Range(0, greekAlphabet.Length);
            prefix = greekAlphabet[index];
            index = Random.Range(0, 10);
            Name = $"{prefix}-{index}";
        }
    }

    /// <summary>
    /// Loops through already created paradigms to see if a name is unique
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool checkIfValidName(string name)
    {
        foreach(Paradigm p in UpgradesOverlayManager.UnlockedParadigms)
        {
            if (p.Name.Equals(name))
                return false;
        }
        return true;
    }
}