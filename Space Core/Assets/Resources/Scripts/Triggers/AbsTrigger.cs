using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsTrigger : MonoBehaviour
{
    public enum ResultID
    {
        Objective,
        Database,
        Skill
    }

    public ResultID Result;
    public string[] ResultContent;

    /// <summary>
    /// When abstracted class is triggered, call is redirected here
    /// </summary>
    protected void handleResult()
    {
        switch(Result)
        {
            case ResultID.Objective:
                LoadObjectives.ProgressObjective(ResultContent[0]);
                break;
            case ResultID.Database:
                LoadDataBaseEntries.UnlockDataEntry(ResultContent[0], ResultContent[1]);
                break;
            case ResultID.Skill:
                //SkillTree.UnlockSkill(ResultContent[0]);
                break;
        }
    }
}
