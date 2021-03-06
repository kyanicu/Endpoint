﻿using System;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

[System.Serializable]
public static class LoadDataBaseEntries
{
    public static Dictionary<string, DataEntry> Logs;

    /// <summary>
    /// DataEntry holds all database panel info and must be unlocked to be displayed in the panel
    /// </summary>
    [Serializable]
    public struct DataEntry
    {
        [Tooltip("The name of the data entry log")]
        public string LogName;

        [Tooltip("The type entry log data")]
        public string LogCategory;

        [Tooltip("The image associated with the data entry log")]
        public string LogImagePath;
        
        [Tooltip("Returns whether or not the log has been unlocked")]
        public bool Visible;

        [Tooltip("All entries associated with this log")]
        public Dictionary<string, Tuple<string, bool>> LogEntries;

        /// <summary>
        /// Data entry constructor. All entries start as locked
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <param name="img"></param>
        /// <param name="cat"></param>
        public DataEntry(string name, Dictionary<string, Tuple<string, bool>> info, string img, string cat)
        {
            LogName = name;
            LogImagePath = img;
            LogEntries = info;
            LogCategory = cat;
            Visible = false;
        }
    }

    /// <summary>
    /// One time process of compiling all database entries from lore text/image files
    /// </summary>
    public static void LoadAllDataEntries()
    {
<<<<<<< HEAD
        Logs = new Dictionary<string, DataEntry>();
        string jsonMap = Resources.Load<TextAsset>("Text/Lore/LoreMap").text;
        JSONNode loreMap = JSON.Parse(jsonMap);
=======
        //Reset log list
        Logs = new Dictionary<string, DataEntry>();

        //Get the directionary holding the category folders
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/Lore");
        string[] categoryFolders = Directory.GetDirectories(dir.FullName);
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

        foreach (KeyValuePair<string, JSONNode> category in (JSONObject)loreMap)
        {

            //for each json node, pull out all of their data entries
            foreach (KeyValuePair<string, JSONNode> subcategory in (JSONObject)category.Value)
            {
                //Load all text assets from resources
                //TextAsset[] dataFolders = Resources.LoadAll<TextAsset>("Text/Lore/" + catDir.Name + "/" + logName);

                //Loop through each text file in log folder

                foreach (JSONNode logName in subcategory.Value)
                {
                    string categoryName = category.Key.Trim('"');
                    string subcategoryName = subcategory.Key.Trim('"');
                    string logNameName = logName.Value.Trim('"');
                    string filePath = $"Text/Lore/{categoryName}/{subcategoryName}/{logNameName}";
                    TextAsset logContent = Resources.Load<TextAsset>(filePath);
                    //Compile the data entry
                    DataEntry d = RetrieveDataEntryFromText(logContent, subcategoryName, categoryName, logNameName);

                    //If it is a new entry, add it to logs
                    if (!Logs.ContainsKey(subcategoryName))
                    {
                        d.Visible = false;
                        Logs.Add(subcategoryName, d);
                    }
                }
            }
        }
    }    

    /// <summary>
    /// Converts a text file into a data entry by retrieving log name, info,
    /// and pulling image from resources with given log name
    /// </summary>
    /// <param name="ft"></param>
    /// <returns></returns>
    public static DataEntry RetrieveDataEntryFromText(TextAsset txt, string logName, string category, string fileType)
    {
        DataEntry entry;

        //Check if data already exists for file name before making new entry
        if (Logs.ContainsKey(logName))
        {
            entry = Logs[logName];
        }
        //If no entry exists compile data under new name
        else
        {
            //Compile new Log 
            entry = new DataEntry();

            entry.Visible = false;

            //Load the respective image from resources/images using the pulled name
            string img = "Images/DataEntryImages/" + logName;
            if (logName == null)
            {
                img ="Images/DataEntryImages/GREENHOUSE 10-2";
                entry.LogImagePath = img;
            }
            entry.LogImagePath = img;
            entry.LogCategory = category;
            entry.LogName = logName;
            entry.LogEntries = new Dictionary<string, Tuple<string, bool>>();
        }

        //Retrieve Log File Type
        string logFileType = fileType;

        //Add the log data into the entry
        entry.LogEntries.Add(logFileType, new Tuple<string, bool>(txt.text, false));

        //Return loaded entry
        return entry;
    }

    /// <summary>
    /// Sets visibility of an entry's article to true
    /// </summary>
    /// <param name="entryName"></param>
    /// <param name="entryArticle"></param>
    public static void UnlockDataEntry(string entryName, string entryArticle)
    {
        //Retrieve the entry
        DataEntry d = Logs[entryName];

        //Set the article's visibility to true
        Tuple<string, bool> newInfo = d.LogEntries[entryArticle];
        newInfo = new Tuple<string, bool>(newInfo.Item1, true);
        d.LogEntries[entryArticle] = newInfo;

        //If entry visibility hasn't been enabled yet, do that
        if (!d.Visible)
        {
            d.Visible = true;
        }

        Logs[entryName] = d;

        //Initiate popp for new entry
        HUDController.instance.InitiateDatabasePopup(entryName, entryArticle);
    }
}
