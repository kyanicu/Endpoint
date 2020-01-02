using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public static class LoadObjectives
{
    //List<Tuple<hasBeenCompleted, Objective>>
    public static List<Tuple<bool, Objective>> PrimaryObjectives;

    //Dictionary<Location Name, List<Objectives>>
    public static Dictionary<string, List<Objective>> SecondaryObjectives;
    
    public static int currentPrimaryObjective;

    public enum SideObjectiveID
    {
        Terminals,
        SaveRooms,
        SecretAreas,
        SpecialEnemies
    }
    private enum LineID
    {
        Description,
        NumToComplete,
        IconType,
        SubDescription
    };

    [Serializable]
    public struct Objective
    {
        public string Name;
        public string Description;
        public string SubDescription;
        public string IconPath;
        private int CurrentlyCompleted;
        private int NumToComplete;

        /// <summary>
        /// Constructs an objective struct on load
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        /// <param name="ntc"></param>
        /// <param name="cc"></param>
        public Objective(string name, string descr, string subdescr, int ntc, string iconType)
        {
            Name = name;
            Description = descr;
            SubDescription = subdescr;
            NumToComplete = ntc;
            CurrentlyCompleted = 0;
            IconPath = "Images/Icons/" + iconType;
        }

        /// <summary>
        /// Increments Objective progress by 1
        /// </summary>
        public void IncrementProgress()
        {
            CurrentlyCompleted++;
        }
        
        /// <summary>
        /// Returns the current objective's progress in a string formatted for description
        /// </summary>
        /// <returns></returns>
        public string CheckCompletion()
        {
            //Returns whether or not objective has been completed
            if (NumToComplete == CurrentlyCompleted)
            {
                return "[DONE]";
            }
            //Ensures primary objectives don't have a counter b/c only one thing to complete
            else if (NumToComplete == 1)
            {
                return "";
            }
            //Returns amount of total completed for objective
            else
            {
                return $"[{CurrentlyCompleted}/{NumToComplete}]";
            }
        }
    }

    /// <summary>
    /// One time process of compiling all objectives from text/image files
    /// </summary>
    public static void LoadAllObjectives()
    {
        currentPrimaryObjective = 0;
        LoadPrimaryObjectives();
        LoadSecondaryObjectives();
    }

    /// <summary>
    /// Retrieves all primary objectives from primary txt files
    /// </summary>
    public static void LoadPrimaryObjectives()
    {
         PrimaryObjectives = new List<Tuple<bool, Objective>>();

        //Get each text file in the primary objectives folder
        TextAsset[] pObjectivesList = Resources.LoadAll<TextAsset>("Text/Objectives/Primary");

        //Loop through each file in folder
        foreach (TextAsset txt in pObjectivesList)
        {
            Objective newPrimary = LoadObjectiveFromText(txt);
            Tuple<bool, Objective> PrimaryObjective = new Tuple<bool, Objective>(false, newPrimary);
            PrimaryObjectives.Add(PrimaryObjective);
        }
    }

    /// <summary>
    /// Retrieves all secondary objectives from primary txt files
    /// </summary>
    public static void LoadSecondaryObjectives()
    {
        SecondaryObjectives = new Dictionary<string, List<Objective>>();

        //Get the directionary holding the location folders
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/Objectives/Secondary");
        string[] locationFolders = Directory.GetDirectories(dir.FullName);

        //Iterate through each location folder
        foreach (string locationPath in locationFolders)
        {
            //Retrieve location name
            string location = GameManager.instance.PullDirectoryEndPoint(locationPath);

            //Get each text file in the location folder
            TextAsset[] pObjectivesList = Resources.LoadAll<TextAsset>("Text/Objectives/Secondary/" + location);

            List<Objective> secondaryObjectivesList = new List<Objective>();

            //Loop through each file in folder and add objectives to locational objective list
            foreach (TextAsset txt in pObjectivesList)
            {
                Objective newSecondary = LoadObjectiveFromText(txt);
                secondaryObjectivesList.Add(newSecondary);
            }

            //Add the newly created list to dictionary with location name as key
            SecondaryObjectives.Add(location, secondaryObjectivesList);
        }
    }

    /// <summary>
    /// Progresses an objective
    /// </summary>
    /// <param name="objName"></param>
    public static void ProgressObjective(string objName)
    {
        //Check if the objective is primary or not
        if (checkIfPrimary(objName))
        {
            int counter = 0;
            //Loop through all primaries
            foreach (Tuple<bool, Objective> primary in PrimaryObjectives)
            {
                Objective primaryObj = primary.Item2;

                //If primary is the one to be completed
                if (primaryObj.Name.Equals(objName.Substring(2)))
                {
                    //Update its item1 to true confirming that its been completed
                    Tuple<bool, Objective> update = new Tuple<bool, Objective>(true, primary.Item2);
                    PrimaryObjectives[counter] = update;
                    UpdateCurrentPrimaryObjectiveID();

                    //Retrieve new primary objective
                    Tuple<bool, Objective> nextPrimObjTuple = PrimaryObjectives[currentPrimaryObjective];
                    Objective nextPrimObj = nextPrimObjTuple.Item2;

                    //Send call to HUD Controller to animate Objective popup
                    HUDController.instance.InitiateDatabasePopup(nextPrimObj.Name, nextPrimObj.Description);
                    break;
                }
                counter++;
            }
        }
        else
        {
            //Retrieve all secondary objectives for this location
            List<Objective> secondaries = SecondaryObjectives[GameManager.Sector];
            List<Objective> update = new List<Objective>();

            //Iterate through secondary objectives
            foreach (Objective obj in secondaries)
            {
                //If objective to update is found
                if (obj.Name.Equals(objName.Substring(2)))
                {
                    //Progress it
                    obj.IncrementProgress();
                }
                update.Add(obj);
            }

            //Save the updated list
            SecondaryObjectives[GameManager.Sector] = update;
        }
    }

    /// <summary>
    /// Loop through all primary objectives and retrieve first objective 
    /// that hasn't been completed yet
    /// </summary>
    public static void UpdateCurrentPrimaryObjectiveID()
    {
        int counter = 0;

        //Loop through all primaries
        foreach (Tuple<bool, Objective> primary in PrimaryObjectives)
        {
            //If objective hasn't been completed yet
            if(!primary.Item1)
            {
                //Update current p obj counter to its index
                currentPrimaryObjective = counter;
                break;
            }
            counter++;
        }
    }

    /// <summary>
    /// Returns whether or not an objective name belongs to a primary objective
    /// </summary>
    /// <returns></returns>
    private static bool checkIfPrimary(string objName)
    {
        //Loop through all primary objectives
        foreach (Tuple<bool, Objective> primary in PrimaryObjectives)
        {
            Objective primaryObj = primary.Item2;

            //If primary is the one to be completed
            if (primaryObj.Name.Equals(objName))
            {
                //Primary objective name matched, return true
                return true;
            }
        }
        //No primary with the given name was found, return false
        return false;
    }

    /// <summary>
    /// Reads and loads an objective given a text file
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private static Objective LoadObjectiveFromText(TextAsset txt)
    {
        //Split text file lines into an array
        string[] fLines = txt.text.Split("\n"[0]);

        //Retrieve Objective info
        string name = txt.name.Substring(2);

        int index = fLines[(int)LineID.Description].IndexOf(": ") + 2;
        string description = fLines[(int)LineID.Description].Substring(index);

        index = fLines[(int)LineID.NumToComplete].IndexOf(": ") + 2;
        string numToConvert = fLines[(int)LineID.NumToComplete].Substring(index);
        numToConvert.Replace("\r", "");
        int numToComplete = int.Parse(numToConvert);

        index = fLines[(int)LineID.IconType].IndexOf(": ") + 2;
        string iconType = fLines[(int)LineID.IconType];

        string subDescription = "";
        for (int x = (int)LineID.SubDescription; x < fLines.Length; x++)
        {
            subDescription += fLines[x] + "\n";
        }

        // public Objective(string name, string descr, string subdescr, int ntc, string iconType, bool iuso)
        return new Objective(name, description, subDescription, numToComplete, iconType);
    }
}
